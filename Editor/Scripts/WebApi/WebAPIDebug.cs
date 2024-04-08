using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        class DebugResponse
        {
            const float k_DebugWaitTime = 2.5f;
            const float k_DebugStreamTime = 4.0f;
            const float k_DebugTotalTime = k_DebugWaitTime + k_DebugStreamTime;

            static readonly string k_DebugStreamResponse = "Particle Systems in Unity are a powerful tool that can simulate fluid entities such as liquids, clouds, and even flames by generating and animating numerous small 2D images in your scenes\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2022.3/Documentation/Manual/Graphics.html\", \"reason\": \"Explains how particle systems are used in simulations\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. The color of these particle systems can significantly impact user engagement, influencing the mood, visual appeal, and overall player experience of your game or application.\n\nThe choice of color for your particle systems largely depends on the context and the desired effect\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2023.2/Documentation/Manual/UIE-uss-built-in-variable-reference.html\", \"reason\": \"Provides additional guidance on choosing the appropriate color\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. Vibrant and contrasting colors can grab attention and create visual interest, enhancing the graphics of your application, irrespective of the platform\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2022.2/Documentation/Manual/Graphics.html\", \"reason\": \"Provides information on how to optimize graphics for visual interest\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. It's beneficial to experiment with different color schemes to see what resonates with your target audience\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2023.2/Documentation/Manual/UIE-uss-built-in-variable-reference.html\", \"reason\": \"Provides guidelines for testing color schemes\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. \n\nHowever, it's also important to consider the overall visual design and branding of your game or application\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity.com/ugs-overview/en/manual/unity-gaming-services-home\", \"reason\": \"Provides information on integrating Unity Gaming Services\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. You might want to leverage principles of color psychology to evoke specific moods or emotions in the player\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2023.2/Documentation/Manual/BestPracticeUnderstandingPerformanceInUnity7.html\", \"reason\": \"Provides a practical tool for implementing color psychology principles\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. Conducting user testing or gathering feedback can be an effective way to determine which color schemes are most engaging for your target audience\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2023.2/Documentation/Manual/best-practice-guides.html\", \"reason\": \"Provides guidance on UI design and development in Unity\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n.\n\nUnity provides various tools and features that allow you to customize and animate the color of your particle systems. The Built-in Particle System and the Visual Effect Graph are two solutions offered by Unity that provide considerable flexibility in terms of color and visual effects\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2023.2/Documentation/Manual/ParticleSystems.html\", \"reason\": \"Provides an overview of particle system solutions in Unity\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. Unity's Particle System module allows for the use of gradients, textures, and the 'Color over Lifetime' function to create dynamic and visually appealing effects\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2020.3/Documentation/Manual/ParticleSystemModules.html\", \"reason\": \"Explains how to change color over the lifetime of particles\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. For more advanced options, Unity's Visual Effect Graph offers a node-based interface for creating stunning particle system visuals\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2022.3/Documentation/Manual/VFXGraph.html\", \"reason\": \"Provides more advanced options for particle system visuals\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n.\n\nFor more in-m_CurrentDepth guidance and inspiration, I highly recommend exploring Unity's documentation and tutorials on particle systems and visual effects\n--boundary-66d876a8d9594209bf18f3ef2e313842\n{\"source\": \"https://docs.unity3d.com/2023.1/Documentation/Manual/ParticleSystems.html\", \"reason\": \"Provides additional resources for more in-m_CurrentDepth guidance on particle systems and visual effects\"}\nboundary-66d876a8d9594209bf18f3ef2e313842\n. These resources can provide you with invaluable knowledge and techniques to make the most out of Unity's particle system and visual effect capabilities.\n";

            float m_DebugSendTime = -1.0f;
            float m_DebugTimeOffset = 0.0f;
            public void Send()
            {
                EditorApplication.update += SendUpdate;
                m_DebugSendTime = Time.realtimeSinceStartup;
            }

            public void CompleteSend()
            {
                EditorApplication.update -= SendUpdate;
            }

            private void SendUpdate()
            {
                m_DebugTimeOffset = Time.realtimeSinceStartup - m_DebugSendTime;
                if (m_DebugTimeOffset >= k_DebugTotalTime)
                {
                    CompleteSend();
                }
            }

            public IWebAPI.RequestStatus RequestStatus
            {
                get
                {
                    if (m_DebugTimeOffset < k_DebugTotalTime)
                        return IWebAPI.RequestStatus.InProgress;
                    else
                        return IWebAPI.RequestStatus.Complete;
                }
            }

            public string GetResponseData()
            {
                if (m_DebugTimeOffset < k_DebugWaitTime)
                    return "";

                var timerPercent = Mathf.Clamp01((m_DebugTimeOffset - k_DebugWaitTime) / k_DebugStreamTime);
                var characterCount = (int)(k_DebugStreamResponse.Length * timerPercent);
                return k_DebugStreamResponse.Substring(0, characterCount);
            }

            ~DebugResponse()
            {
                CompleteSend();
            }
        }
        DebugResponse m_DebugMessage;
    }
}
