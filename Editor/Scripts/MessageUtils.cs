using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat
{
    internal static class MessageUtils
    {
        public const string k_Today = "000000#Today";
        public const string k_Yesterday = "000001#Yesterday";
        public const string k_ThisWeek = "000003#This Week";
        public const string k_ThisMonth = "000004#This Month";

        const string k_BoundaryRegexComplete = @"\r?\n--boundary-\w{32}\r?\n({[\s\S]*?})\r?\nboundary-\w{32}\r?\n";
        const string k_BoundaryRegexBegin = @"\r?\n--boundary-\w{32}\r?\n";

        static readonly Regex k_FootnoteInlineRegex = new(@"\[\^(\d+)\^\](?=(?:\[\^(?:\d+)\^\])*\.(?:\s|\r))", RegexOptions.Compiled);
        static readonly Regex k_FootnoteURLsRegex = new( @"(\r\n|\n)\[?\^(\d+)\^\]?: \(source: \[(.*?)\]\((.*?)\)\)", RegexOptions.Compiled);
        static readonly Regex k_FootnoteURLsOldFormatRegex = new( @"(\r\n|\n)(?:> )?\[\^(\d+)\^\]: \[(.*?)\]\((.*?)\)", RegexOptions.Compiled);
        static readonly Regex k_FootnoteURLsNoTitleRegex = new( @"(\r\n|\n)\[\^(\d+)\^\]: \((.*?)\)", RegexOptions.Compiled);

        static readonly Regex k_FootnoteInvalidInlineRegex = new(@"\s*(\[(\d+)\])+(?=[:.]($|\s|\n))", RegexOptions.Compiled);

        static readonly Regex k_SourceMarkerRegex = new(@"{{source:(\d+)}}", RegexOptions.Compiled);
        static readonly Regex k_DoubleSourceMarkerRegex = new(@"{{source:(\d+)}}{{source:(\d+)}}", RegexOptions.Compiled);

        static readonly Regex k_BoldRegex = new(@"\*\*(.*?)\*\*", RegexOptions.Compiled);
        static readonly Regex k_ContextRegex = new($"{MuseChatConstants.ContextTagEscaped}(.*){MuseChatConstants.ContextTagEscaped}", RegexOptions.Compiled | RegexOptions.Singleline );

        static readonly DayOfWeek k_FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        static StringBuilder s_stringBuilder = new();

        struct SourceOrFootnote
        {
            public bool IsSource;
            public int FootnoteIndex;
            public int SourceIndex;
            public int ForwardTo;
            public int FinalSourceIndex;
            public string Title;
            public string URL;
        };

        static string ProcessChunk(string chunk)
        {
            if (string.IsNullOrEmpty(chunk))
            {
                return chunk;
            }

            var matches = k_BoldRegex.Matches(chunk);
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var boldText = match.Groups[1].Value;
                chunk = chunk.Replace(match.Value, $"<b>{boldText}</b>");
            }

            return chunk;
        }

        public static void ProcessText(MuseMessage message, ref IList<WebAPI.SourceBlock> sourceBlocks, out string messageContent)
        {
            List<SourceOrFootnote> sourceOrFootnotes = new();

            messageContent = message.Content;
            if (message.Role != MuseEditorDriver.k_AssistantRole)
            {
                var contextBlock = k_ContextRegex.Match(messageContent);
                if (contextBlock.Success)
                {
                    messageContent = messageContent.Replace(contextBlock.Value, string.Empty);
                }

                return;
            }

            s_stringBuilder.Clear();

            var chunks = Regex.Split(message.Content, k_BoundaryRegexComplete);

            for (var i = 0; i < chunks.Length; i += 2)
            {
                // Even chunk - text
                // If this is the last chunk, we chop off the last word unless the message is complete
                // If the starting block regex is here, we don't use any of that text
                var lastBlock = (i == chunks.Length - 1);
                string text;
                if (lastBlock && !message.IsComplete)
                {
                    var subChunks = Regex.Split(chunks[i], k_BoundaryRegexBegin);

                    if (subChunks.Length < 2)
                    {
                        int lastSpace = subChunks[0].LastIndexOf(' ');

                        if (lastSpace > 0)
                        {
                            text = ProcessChunk(subChunks[0].Substring(0, lastSpace));
                        }
                        else
                        {
                            text = ProcessChunk(subChunks[0]);
                        }
                    }
                    else
                    {
                        text = ProcessChunk(subChunks[0]);
                    }
                }
                else
                {
                    text = ProcessChunk(chunks[i]);
                }

                if (message.IsComplete)
                {
                    // Replace inline footnotes with placeholders
                    text = k_FootnoteInlineRegex.Replace(text, match =>
                    {
                        int index = int.Parse(match.Groups[1].Value);
                        sourceOrFootnotes.Add(new SourceOrFootnote() { IsSource = false, FootnoteIndex = index });

                        return $"{{{{source:{sourceOrFootnotes.Count}}}}}";
                    });
                }

                s_stringBuilder.Append(text);

                // If this is not the last chunk placeholder source index
                if (!lastBlock)
                {
                    if (message.IsComplete)
                    {
                        // Replace source (boundary tag) with placeholders
                        sourceOrFootnotes.Add(new SourceOrFootnote() { IsSource = true, FootnoteIndex = 0, SourceIndex = i / 2 });

                        s_stringBuilder.Append($"{{{{source:{sourceOrFootnotes.Count}}}}}");
                    }
                    else
                    {
                        s_stringBuilder.Append(GetReferenceSpriteString(i / 2 + 1));
                    }
                }
            }

            for (var i = 1; i < chunks.Length; i += 2)
            {
                // Odd chunk, reference block
                // If the message is complete, store all this data so we can display the source links
                // Otherwise, ignore it
                if (message.IsComplete)
                {
                    try
                    {
                        WebAPI.SourceBlock sourceBlock = JsonUtility.FromJson<WebAPI.SourceBlock>(chunks[i]);

                        var sourceEntryIndex = sourceOrFootnotes.FindIndex(e => e.IsSource && e.SourceIndex == i/2);
                        if (sourceEntryIndex != -1)
                        {
                            var sourceEntry = sourceOrFootnotes[sourceEntryIndex];
                            sourceEntry.Title = sourceBlock.reason;
                            sourceEntry.URL = sourceBlock.source;
                            sourceOrFootnotes[sourceEntryIndex] = sourceEntry;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to parse source block: {e}");
                    }
                }
            }

            // Reliably parsing footnotes only works with complete messages with footnotes
            if (message.IsComplete && sourceOrFootnotes.Count > 0)
            {
                if (sourceBlocks == null)
                {
                    sourceBlocks = new List<WebAPI.SourceBlock>();
                }

                messageContent = s_stringBuilder.ToString();

                // Fill in footnote title/URL found at end of text
                messageContent = k_FootnoteURLsRegex.Replace(messageContent, match =>
                {
                    AddFootnoteAsSource(match, sourceOrFootnotes);
                    return "";
                });
                messageContent = k_FootnoteURLsOldFormatRegex.Replace(messageContent, match =>
                {
                    AddFootnoteAsSource(match, sourceOrFootnotes);
                    return "";
                });
                messageContent = k_FootnoteURLsNoTitleRegex.Replace(messageContent, match =>
                {
                    AddFootnoteAsSource(match, sourceOrFootnotes, true);
                    return "";
                });

                // Create SourceBlocks from all source and footnote information
                for (int i = 0; i < sourceOrFootnotes.Count; i++)
                {
                    var footnoteInfo = sourceOrFootnotes[i];
                    if (!string.IsNullOrEmpty(footnoteInfo.URL))
                    {
                        var duplicateURLIndex = sourceOrFootnotes.FindIndex(e =>
                            !string.IsNullOrEmpty(e.URL) && e.URL.Equals(footnoteInfo.URL));

                        if (duplicateURLIndex != -1 && duplicateURLIndex < i)
                        {
                            footnoteInfo.FinalSourceIndex = sourceOrFootnotes[duplicateURLIndex].FinalSourceIndex;
                        }
                        else
                        {
                            sourceBlocks.Add(new WebAPI.SourceBlock()
                            {
                                reason = footnoteInfo.Title,
                                source = footnoteInfo.URL
                            });

                            footnoteInfo.FinalSourceIndex = sourceBlocks.Count;
                        }

                        sourceOrFootnotes[i] = footnoteInfo;
                    }
                }

                // Remove duplicate placeholders that effectively refer to the same source
                bool removeDuplicates = true;
                while (removeDuplicates)
                {
                    removeDuplicates = false;

                    messageContent = k_DoubleSourceMarkerRegex.Replace(messageContent, match =>
                    {
                        int index1 = int.Parse(match.Groups[1].Value);
                        int index2 = int.Parse(match.Groups[2].Value);
                        var footnoteInfo1 = sourceOrFootnotes[index1 - 1];
                        var footnoteInfo2 = sourceOrFootnotes[index2 - 1];

                        if (footnoteInfo1.FinalSourceIndex == footnoteInfo2.FinalSourceIndex)
                        {
                            // Rerun afterwards in case we had more then one duplicate in a row
                            removeDuplicates = true;

                            return $"{{{{source:{index1}}}}}";
                        }

                        return match.Value;
                    });
                }

                // Replace all placeholders with sprites/indices
                messageContent = k_SourceMarkerRegex.Replace(messageContent, match =>
                {
                    int index = int.Parse(match.Groups[1].Value);
                    var footnoteInfo = sourceOrFootnotes[index-1];

                    if (footnoteInfo.FinalSourceIndex == 0)
                    {
                        return "";
                    }

                    return GetReferenceSpriteString(footnoteInfo.FinalSourceIndex);
                });
            }
            else
            {
                messageContent = s_stringBuilder.ToString();
            }

            // Replace invalid non-footnote markers
            messageContent = k_FootnoteInvalidInlineRegex.Replace(messageContent, "");
        }

        private static void AddFootnoteAsSource(Match match, List<SourceOrFootnote> sourceOrFootnotes, bool noTitle = false)
        {
            int index = int.Parse(match.Groups[2].Value);
            var title = noTitle ? "Source" : match.Groups[3].Value;
            var URL = match.Groups[noTitle ? 3 : 4].Value;

            var sourceEntryIndex = sourceOrFootnotes.FindIndex(e => !e.IsSource && e.FootnoteIndex == index);
            if (sourceEntryIndex != -1)
            {
                var sourceEntry = sourceOrFootnotes[sourceEntryIndex];
                sourceEntry.Title = title;
                sourceEntry.URL = URL;
                sourceOrFootnotes[sourceEntryIndex] = sourceEntry;
            }
        }

        public static string GetReferenceSpriteString(int index)
        {
            return $" <sprite=\"{MuseChatConstants.ReferenceSprite}\" index={index}>";
        }

        public static string GetAssetLink<T>(string guid, string title)
        {
            // d_GameObject Icon
            // d_SceneAsset Icon
            return $"<link=\"{guid}\"><u>{title}</u></link>";
        }

        public static bool GetAssetFromLink(string linkUrl, out Object asset)
        {
            asset = null;
            try
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(linkUrl);
                if (string.IsNullOrEmpty(assetPath))
                {
                    return false;
                }

                var assetType = AssetDatabase.GetMainAssetTypeFromGUID(new GUID(linkUrl));
                if (assetType == default)
                {
                    return false;
                }

                asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);
                return asset != null;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load asset from link: " + e);
                asset = null;
                return false;
            }
        }

        public static string GetWebLink(string url, string title)
        {
            return $"<a href=\"{url}\">{title}</a>";
        }

        public static string GetTextWithMaxLength(this string text, int maxLength)
        {
            if (maxLength <= MuseChatConstants.TextCutoffSuffix.Length)
            {
                throw new ArgumentException("Max length must be greater than " + MuseChatConstants.TextCutoffSuffix.Length);
            }

            if (text.Length <= maxLength)
            {
                return text;
            }

            maxLength -= MuseChatConstants.TextCutoffSuffix.Length;
            return text.Substring(0, Mathf.Min(maxLength, text.Length)) + MuseChatConstants.TextCutoffSuffix;
        }

        public static string GetMessageTimestampGroup(long timeStampRaw, long nowRaw)
        {
            var nowTime = DateTimeOffset.FromUnixTimeMilliseconds(nowRaw);
            var yesterdayTime = nowTime.AddDays(-1);
            var timeStamp = DateTimeOffset.FromUnixTimeMilliseconds(timeStampRaw);
            var timeDiff = nowTime - timeStamp;
            if (timeDiff.Days <= 1)
            {
                if (nowTime.Day == timeStamp.Day)
                {
                    return k_Today;
                }

                if (yesterdayTime.DayOfYear == timeStamp.DayOfYear)
                {
                    return k_Yesterday;
                }
            }

            var startOfWeekNow = nowTime.AddDays(-(int)nowTime.DayOfWeek + (int)k_FirstDayOfWeek);
            var startOfWeekTimeStamp = timeStamp.AddDays(-(int)timeStamp.DayOfWeek + (int)k_FirstDayOfWeek);

            if (startOfWeekNow.Date == startOfWeekTimeStamp.Date)
                return k_ThisWeek;

            if (nowTime.Month == timeStamp.Month && nowTime.Year == timeStamp.Year)
                return k_ThisMonth;

            string yearMonthKey = $"{5000 - timeStamp.Year}{50 - timeStamp.Month}";
            return $"{yearMonthKey}#{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(timeStamp.Month)} {timeStamp.Year}";
        }
    }
}
