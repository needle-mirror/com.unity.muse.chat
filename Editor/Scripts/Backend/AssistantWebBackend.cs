using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Model;
using UnityEngine;

namespace Unity.Muse.Chat
{
    internal class AssistantWebBackend : IAssistantBackend
    {
        static readonly TimeSpan k_CancellationTimeout = TimeSpan.FromSeconds(30);

        CancellationTokenSource m_CancellationToken;

        /// <summary>
        /// The WebAPI implementation used to communicate with the Muse Backend.
        /// </summary>
        readonly WebAPI k_WebAPI = new();

        public bool SessionStatusTrackingEnabled => true;

        public bool RequestInProgress => m_CancellationToken != null;

        public void Cancel()
        {
            m_CancellationToken?.Cancel();
        }

        /// <summary>
        /// Starts a request to refresh the list of conversations available. This is non-blocking.
        /// </summary>
        public Task<IEnumerable<MuseConversationInfo>> ConversationRefresh(CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<IEnumerable<MuseConversationInfo>>();

            k_WebAPI.GetConversations(
                EditorLoopUtilities.EditorLoopRegistration,
                x =>
                {
                    var result = new List<MuseConversationInfo>();
                    foreach (var remoteInfo in x)
                    {
                        var localInfo = new MuseConversationInfo
                        {
                            Id = new(remoteInfo.ConversationId),
                            Title = remoteInfo.Title,
                            LastMessageTimestamp = remoteInfo.LastMessageTimestamp,
                            IsContextual = remoteInfo.IsContextual,
                            IsFavorite = remoteInfo.IsFavorite != null && remoteInfo.IsFavorite.Value
                        };

                        result.Add(localInfo);
                    }

                    tsc.SetResult(result);
                }, exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                }
            );

            return tsc.Task;
        }

        /// <summary>
        /// Starts a webrequest that attempts to load the conversation with <see cref="conversationId"/>.
        /// </summary>
        /// <param name="conversationId">If not null or empty function acts as noop.</param>
        /// <param name="callback"></param>
        public Task<MuseConversation> ConversationLoad(MuseConversationId conversationId, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<MuseConversation>();

            if (!conversationId.IsValid)
            {
                tsc.SetException(new Exception("Invalid conversation ID"));
                return tsc.Task;
            }

            k_WebAPI.GetConversation(
                conversationId.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                x =>
                {
                    tsc.SetResult(ConvertConversation(x));
                }, exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                }
            );

            return tsc.Task;
        }

        /// <summary>
        /// Starts a webrequest that attempts to rename change the favorite state of a conversation with <see cref="conversationId"/>.
        /// </summary>
        /// <param name="conversationId">If not null or empty function acts as noop.</param>
        /// <param name="isFavorite">New favorite state of the conversation</param>
        public Task ConversationFavoriteToggle(MuseConversationId conversationId, bool isFavorite, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            if (!conversationId.IsValid)
            {
                tsc.SetException(new Exception("Invalid conversation ID"));
                return tsc.Task;
            }

            k_WebAPI.SetConversationFavoriteState(conversationId.Value,
                isFavorite,
                EditorLoopUtilities.EditorLoopRegistration,
                () => tsc.SetResult(true),
                exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                });

            return tsc.Task;
        }

        public async Task<MuseConversationId> ConversationCreate(CancellationToken ct = default)
        {
            var conversation = await k_WebAPI.PostConversation(MuseChatState.FunctionCache.AllFunctionDefinitions);
            return new(conversation.Id);
        }

        /// <summary>
        /// Starts a webrequest that attempts to rename a conversation with <see cref="conversationId"/>.
        /// </summary>
        /// <param name="conversationId">If not null or empty function acts as noop.</param>
        /// <param name="newName">New name of the conversation</param>
        /// <param name="onComplete">Callback when the rename operation is complete</param>
        public Task ConversationRename(MuseConversationId conversationId, string newName, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            if (!conversationId.IsValid)
            {
                tsc.SetException(new Exception("Invalid conversation ID"));
                return tsc.Task;
            }

            k_WebAPI.RenameConversation(conversationId.Value,
                newName,
                EditorLoopUtilities.EditorLoopRegistration,
                () => tsc.SetResult(true), exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                });

            return tsc.Task;
        }

        public Task ConversationSetAutoTitle(MuseConversationId id, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            if (!id.IsValid)
            {
                tsc.SetException(new Exception("Invalid conversation ID"));
                return tsc.Task;
            }

            var sc = SynchronizationContext.Current;

            k_WebAPI.GetConversationTitle(id.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                suggestedTitle =>
                {
                    if (!string.IsNullOrEmpty(suggestedTitle))
                    {
                        sc.Post( async _ =>
                            {
                                await ConversationRename(id, suggestedTitle.Trim('"'), ct);
                                tsc.SetResult(true);
                            }, null);
                    }
                }, exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                });
            return tsc.Task;
        }

        /// <summary>
        /// Starts a webrequest that attempts to delete a conversation with <see cref="conversation"/>.
        /// </summary>
        /// <param name="conversation">If not null or empty function acts as noop.</param>
        /// <param name="onComplete"></param>
        public Task ConversationDelete(MuseConversationInfo conversation, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            if (!conversation.Id.IsValid)
            {
                tsc.SetException(new Exception("Invalid conversation ID"));
                return tsc.Task;
            }

            k_WebAPI.DeleteConversation(
                conversation.Id.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                () => tsc.SetResult(true), exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                }
            );

            return tsc.Task;
        }

        public async Task ConversationDeleteFragment(MuseConversationId conversationId, string fragment, CancellationToken ct = default)
        {
            await k_WebAPI.DeleteConversationFragment(conversationId, fragment);
        }

        /// <summary>
        /// Starts a request to refresh the list of conversations available. This is non-blocking.
        /// </summary>
        public Task<IEnumerable<MuseChatInspiration>> InspirationRefresh(CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<IEnumerable<MuseChatInspiration>>();

            k_WebAPI.GetInspirations(
                EditorLoopUtilities.EditorLoopRegistration,
                x =>
                {
                    var result = new List<MuseChatInspiration>();
                    foreach (var entry in x)
                    {
                        result.Add(entry.ToInternal());
                    }

                    tsc.SetResult(result);
                }, exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                }
            );

            return tsc.Task;
        }

        /// <summary>
        /// Starts a webrequest that attempts to add or update a inspiration.
        /// </summary>
        /// <param name="inspiration">the inspiration data to update.</param>
        public Task InspirationUpdate(MuseChatInspiration inspiration, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            var externalData = inspiration.ToExternal();
            if (!inspiration.Id.IsValid)
            {
                externalData.Id = null;
                k_WebAPI.AddInspiration(externalData,
                    EditorLoopUtilities.EditorLoopRegistration,
                    _ =>
                    {
                        tsc.SetResult(true);
                    }, exception =>
                    {
                        Debug.LogException(exception);
                        tsc.SetException(exception);
                    });
            }
            else
            {
                k_WebAPI.UpdateInspiration(externalData,
                    EditorLoopUtilities.EditorLoopRegistration,
                    _ => tsc.SetResult(true),
                    exception =>
                    {
                        Debug.LogException(exception);
                        tsc.SetException(exception);
                    });
            }

            return tsc.Task;
        }

        /// <summary>
        /// Starts a webrequest that attempts to delete an inspiration.
        /// </summary>
        /// <param name="inspiration">the inspiration data to delete.</param>
        public Task InspirationDelete(MuseChatInspiration inspiration, CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            var externalData = inspiration.ToExternal();
            if (!inspiration.Id.IsValid)
            {
                tsc.SetException(new InvalidOperationException("Tried to delete non-existing Inspiration entry!"));
                return tsc.Task;
            }

            k_WebAPI.DeleteInspiration(externalData,
                EditorLoopUtilities.EditorLoopRegistration,
                _ => tsc.SetResult(true), exception =>
                {
                    Debug.LogException(exception);
                    tsc.SetException(exception);
                });
            return tsc.Task;
        }

        public Task SendFeedback(MuseConversationId conversationId, MessageFeedback feedback, CancellationToken ct = default)
        {
            k_WebAPI.SendFeedback(feedback.Message, conversationId.Value, feedback.MessageId.FragmentId, feedback.Sentiment, feedback.Type);
            return Task.CompletedTask;
        }

        public Task<bool> CheckEntitlement(CancellationToken ct = default)
        {
            var tsc = new TaskCompletionSource<bool>();

            k_WebAPI.CheckBetaEntitlement(
                EditorLoopUtilities.EditorLoopRegistration,
                tsc.SetResult);

            return tsc.Task;
        }

        public Task<MuseChatStreamHandler> SendPrompt(MuseConversationId conversationId, string prompt, EditorContextReport context, ChatCommandType commandType, List<MuseChatContextEntry> selectionContext, CancellationToken ct = default)
        {
            var stream = k_WebAPI.BuildChatStream(
                prompt,
                conversationId.Value,
                context,
                commandType,
                selectionContext: ToExternalContext(selectionContext)
            );

            return Task.FromResult(stream);
        }

        public async Task<SmartContextResponse> SendSmartContext(MuseConversationId conversationId, string prompt, EditorContextReport context, CancellationToken ct = default)
        {
            var timeout = new CancellationTokenSource(k_CancellationTimeout);
            var merged = CancellationTokenSource.CreateLinkedTokenSource(ct, timeout.Token);

            return await k_WebAPI.PostSmartContextAsync(prompt, context,
                MuseChatState.SmartContextToolbox.Tools.Select(c => c.FunctionDefinition).ToList(),
                conversationId.Value,
                merged.Token);
        }

        public async Task<object> RepairCode(MuseConversationId conversationId, int messageIndex, string errorToRepair, string scriptToRepair, ScriptType scriptType, CancellationToken ct = default)
        {
            var timeout = new CancellationTokenSource(k_CancellationTimeout);
            var merged = CancellationTokenSource.CreateLinkedTokenSource(ct, timeout.Token);

            return await k_WebAPI.CodeRepair(conversationID: conversationId.Value,
                messageIndex: messageIndex,
                errorToRepair: errorToRepair,
                scriptToRepair: scriptToRepair,
                cancellationToken: merged.Token,
                scriptType: scriptType);
        }

        public async Task EditCode(MuseMessageId messageId, string newContent, CancellationToken ct = default)
        {
            var timeout = new CancellationTokenSource(k_CancellationTimeout);
            var merged = CancellationTokenSource.CreateLinkedTokenSource(ct, timeout.Token);

            await k_WebAPI.EditContent(conversationId: messageId.ConversationId.Value,
                fragmentId: messageId.FragmentId,
                newContent: newContent);
        }

        public async Task<List<VersionSupportInfo>> GetVersionSupportInfo(string version, CancellationToken ct = default)
        {
            var response = await k_WebAPI.GetServerCompatibility(version);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data;
            else
                return null;
        }

        MuseConversation ConvertConversation(ClientConversation remoteConversation)
        {
            var conversationId = new MuseConversationId(remoteConversation.Id);
            MuseConversation localConversation = new()
            {
                Id = conversationId,
                Title = remoteConversation.Title
            };

            for (var i = 0; i < remoteConversation.History.Count; i++)
            {
                var fragment = remoteConversation.History[i];
                var message = new MuseMessage
                {
                    Id = new(conversationId, fragment.Id, MuseMessageIdType.External),
                    IsComplete = true,
                    Role = fragment.Role,
                    Author = fragment.Author,
                    Content = fragment.Content,
                    Timestamp = fragment.Timestamp,
                    Context = ConvertSelectionContextToInternal(fragment.SelectedContextMetadata),
                    MessageIndex = i
                };

                localConversation.Messages.Add(message);
            }

            return localConversation;
        }

        public static MuseChatContextEntry[] ConvertSelectionContextToInternal(List<SelectedContextMetadataItems> context)
        {
            if (context == null || context.Count == 0)
            {
                return Array.Empty<MuseChatContextEntry>();
            }

            var result = new MuseChatContextEntry[context.Count];
            for (var i = 0; i < context.Count; i++)
            {
                var entry = context[i];
                if (entry.EntryType == null)
                {
                    // Invalid entry
                    Debug.LogError("Invalid Selection Context Entry");
                    continue;
                }

                var entryType = (MuseChatContextType)entry.EntryType;
                switch (entryType)
                {
                    case MuseChatContextType.ConsoleMessage:
                    {
                        result[i] = new MuseChatContextEntry
                        {
                            EntryType = MuseChatContextType.ConsoleMessage,
                            Value = entry.Value,
                            ValueType = entry.ValueType
                        };

                        break;
                    }

                    default:
                    {
                        result[i] = new MuseChatContextEntry
                        {
                            Value = entry.Value,
                            DisplayValue = entry.DisplayValue,
                            EntryType = entryType,
                            ValueType = entry.ValueType,
                            ValueIndex = entry.ValueIndex ?? 0
                        };

                        break;
                    }
                }
            }

            return result;
        }

        static List<SelectedContextMetadataItems> ToExternalContext(List<MuseChatContextEntry> internalContext)
        {
            if (internalContext == null || internalContext.Count == 0)
            {
                return null;
            }

            var result = new List<SelectedContextMetadataItems>();
            for (var i = 0; i < internalContext.Count; i++)
            {
                var entry = internalContext[i];
                result.Add(new SelectedContextMetadataItems
                {
                    DisplayValue = entry.DisplayValue,
                    EntryType = (int)entry.EntryType,
                    Value = entry.Value,
                    ValueIndex = entry.ValueIndex,
                    ValueType = entry.ValueType
                });
            }

            return result;
        }
    }
}
