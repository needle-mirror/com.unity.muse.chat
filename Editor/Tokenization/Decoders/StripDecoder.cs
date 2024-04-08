using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization.Decoders
{
    /// <summary>
    ///     Strip Decoder removes certain char from the substring of the token in the list.
    /// </summary>
    class StripDecoder : DecoderBase
    {
        readonly char m_Content;
        readonly int m_End;
        readonly int m_Start;

        public StripDecoder(char content, int start, int end)
        {
            m_Content = content;
            m_Start = start;
            m_End = end;
        }

        protected override void DecodeInternal(IEnumerable<string> tokens, IOutput<string> output)
        {
            foreach (var token in tokens)
            {
                var startCut = 0;
                for (var i = 0; i < Math.Min(m_Start, token.Length); i++)
                    if (token[i] == m_Content)
                        startCut = i + 1;
                    else
                        break;

                var stopCut = token.Length;
                for (var i = 0; i < m_End; i++)
                {
                    var index = token.Length - i - 1;
                    if (index < 0)
                        break;

                    if (token[index] == m_Content)
                        stopCut = index;
                    else
                        break;
                }

                output.Write(token.Substring(startCut, stopCut - startCut));
            }
        }
    }
}
