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
                    result.Mode = ChatCommandType.Run;
                    break;
                }

                case Inspiration.ModeEnum.Code:
                {
                    result.Mode = ChatCommandType.Code;
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
            Inspiration apiData;
            apiData = data.Id.IsValid
                ? new Inspiration(id: data.Id.Value, value: data.Value, description: data.Description)
                : new Inspiration(value: data.Value, description: data.Description);

            switch (data.Mode)
            {
                case ChatCommandType.Ask:
                {
                    apiData.Mode = Inspiration.ModeEnum.Ask;
                    break;
                }

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
            }

            return apiData;
        }
    }
}
