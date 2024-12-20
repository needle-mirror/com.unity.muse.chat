using System;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    internal static class MuseChatDataUtils
    {
        public static MuseChatInspiration ToInternal(this Inspiration apiData)
        {
            var result = new MuseChatInspiration
            {
                Id = new MuseInspirationId(apiData.Id),
                Description = apiData.Description,
                Value = apiData.Value
            };

            switch (apiData.Mode)
            {
                case Inspiration.ModeEnum.Ask:
                {
                    result.Mode = ChatCommandType.Ask;
                    break;
                }

                case Inspiration.ModeEnum.Run:
                {
#if ENABLE_ASSISTANT_BETA_FEATURES
                    result.Mode = ChatCommandType.Run;
#endif
                    break;
                }

                case Inspiration.ModeEnum.Code:
                {
#if ENABLE_ASSISTANT_BETA_FEATURES
                    result.Mode = ChatCommandType.Code;
#endif
                    break;
                }

                default:
                {
                    throw new InvalidOperationException();
                }
            }

            return result;
        }

        public static Inspiration ToExternal(this MuseChatInspiration data)
        {
            Inspiration apiData = new Inspiration(Inspiration.ModeEnum.Ask, data.Value)
            {
                Id = data.Id.IsValid ? data.Id.Value : default,
                Description = data.Description
            };

            switch (data.Mode)
            {
                case ChatCommandType.Ask:
                {
                    apiData.Mode = Inspiration.ModeEnum.Ask;
                    break;
                }
#if ENABLE_ASSISTANT_BETA_FEATURES
                case ChatCommandType.Run:
                {
                    apiData.Mode = Inspiration.ModeEnum.Run;
                    break;
                }

                case ChatCommandType.Code:
                {
                    apiData.Mode = Inspiration.ModeEnum.Code;
                    break;
                }
#endif
            }

            return apiData;
        }
    }
}
