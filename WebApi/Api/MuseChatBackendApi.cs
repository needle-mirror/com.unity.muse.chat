using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat.BackendApi.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    internal interface IMuseChatBackendApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentId(string conversationId, string fragmentId);

        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfo(string conversationId, string fragmentId);
        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ErrorResponse</returns>
        ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1(string conversationId, string fragmentId);

        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfo(string conversationId, string fragmentId);
        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationId(string conversationId);

        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationIdWithHttpInfo(string conversationId);
        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ErrorResponse</returns>
        ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationIdV1(string conversationId);

        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationIdV1WithHttpInfo(string conversationId);
        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> DeleteMuseConversationsByTags(List<string> tags = default(List<string>));

        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> DeleteMuseConversationsByTagsWithHttpInfo(List<string> tags = default(List<string>));
        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ErrorResponse</returns>
        ApiResponse<ErrorResponse> DeleteMuseConversationsByTagsV1(List<string> tags = default(List<string>));

        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        ApiResponse<ErrorResponse> DeleteMuseConversationsByTagsV1WithHttpInfo(List<string> tags = default(List<string>));
        /// <summary>
        /// Health
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Object</returns>
        ApiResponse<Object> GetHealth();

        /// <summary>
        /// Health
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> GetHealthWithHttpInfo();
        /// <summary>
        /// Healthz
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Object</returns>
        ApiResponse<Object> GetHealthz();

        /// <summary>
        /// Healthz
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> GetHealthzWithHttpInfo();
        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>List&lt;ConversationInfo&gt;</returns>
        [Obsolete]
        ApiResponse<List<ConversationInfo>> GetMuseConversation(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?));

        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of List&lt;ConversationInfo&gt;</returns>
        [Obsolete]
        ApiResponse<List<ConversationInfo>> GetMuseConversationWithHttpInfo(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?));
        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ResponseGetMuseConversationUsingConversationId</returns>
        [Obsolete]
        ApiResponse<ResponseGetMuseConversationUsingConversationId> GetMuseConversationUsingConversationId(string conversationId);

        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ResponseGetMuseConversationUsingConversationId</returns>
        [Obsolete]
        ApiResponse<ResponseGetMuseConversationUsingConversationId> GetMuseConversationUsingConversationIdWithHttpInfo(string conversationId);
        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ResponseGetMuseConversationUsingConversationIdV1</returns>
        ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> GetMuseConversationUsingConversationIdV1(string conversationId);

        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ResponseGetMuseConversationUsingConversationIdV1</returns>
        ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> GetMuseConversationUsingConversationIdV1WithHttpInfo(string conversationId);
        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>List&lt;ConversationInfo&gt;</returns>
        ApiResponse<List<ConversationInfo>> GetMuseConversationV1(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?));

        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of List&lt;ConversationInfo&gt;</returns>
        ApiResponse<List<ConversationInfo>> GetMuseConversationV1WithHttpInfo(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?));
        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ResponseGetMuseInspiration</returns>
        [Obsolete]
        ApiResponse<ResponseGetMuseInspiration> GetMuseInspiration(string mode = default(string), int? limit = default(int?), int? skip = default(int?));

        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of ResponseGetMuseInspiration</returns>
        [Obsolete]
        ApiResponse<ResponseGetMuseInspiration> GetMuseInspirationWithHttpInfo(string mode = default(string), int? limit = default(int?), int? skip = default(int?));
        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ResponseGetMuseInspirationV1</returns>
        ApiResponse<ResponseGetMuseInspirationV1> GetMuseInspirationV1(string mode = default(string), int? limit = default(int?), int? skip = default(int?));

        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of ResponseGetMuseInspirationV1</returns>
        ApiResponse<ResponseGetMuseInspirationV1> GetMuseInspirationV1WithHttpInfo(string mode = default(string), int? limit = default(int?), int? skip = default(int?));
        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Dictionary&lt;string, OptDecision&gt;</returns>
        [Obsolete]
        ApiResponse<Dictionary<string, OptDecision>> GetMuseOpt();

        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Dictionary&lt;string, OptDecision&gt;</returns>
        [Obsolete]
        ApiResponse<Dictionary<string, OptDecision>> GetMuseOptWithHttpInfo();
        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Dictionary&lt;string, OptDecision&gt;</returns>
        ApiResponse<Dictionary<string, OptDecision>> GetMuseOptV1();

        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Dictionary&lt;string, OptDecision&gt;</returns>
        ApiResponse<Dictionary<string, OptDecision>> GetMuseOptV1WithHttpInfo();
        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>string</returns>
        [Obsolete]
        ApiResponse<string> GetMuseTopicUsingConversationId(string conversationId, string organizationId);

        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>ApiResponse of string</returns>
        [Obsolete]
        ApiResponse<string> GetMuseTopicUsingConversationIdWithHttpInfo(string conversationId, string organizationId);
        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>string</returns>
        ApiResponse<string> GetMuseTopicUsingConversationIdV1(string conversationId, string organizationId);

        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>ApiResponse of string</returns>
        ApiResponse<string> GetMuseTopicUsingConversationIdV1WithHttpInfo(string conversationId, string organizationId);
        /// <summary>
        /// Health Head
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Object</returns>
        ApiResponse<Object> HeadHealth();

        /// <summary>
        /// Health Head
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> HeadHealthWithHttpInfo();
        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentId(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch);

        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfo(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch);
        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ErrorResponse</returns>
        ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch);

        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfo(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch);
        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationId(string conversationId, ConversationPatchRequest conversationPatchRequest);

        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationIdWithHttpInfo(string conversationId, ConversationPatchRequest conversationPatchRequest);
        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ErrorResponse</returns>
        ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationIdV1(string conversationId, ConversationPatchRequest conversationPatchRequest);

        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationIdV1WithHttpInfo(string conversationId, ConversationPatchRequest conversationPatchRequest);
        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseAgentAction(ActionRequest actionRequest);

        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseAgentActionWithHttpInfo(ActionRequest actionRequest);
        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>Object</returns>
        ApiResponse<Object> PostMuseAgentActionV1(ActionRequest actionRequest);

        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> PostMuseAgentActionV1WithHttpInfo(ActionRequest actionRequest);
        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseAgentCodeRepair(ActionCodeRepairRequest actionCodeRepairRequest);

        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseAgentCodeRepairWithHttpInfo(ActionCodeRepairRequest actionCodeRepairRequest);
        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>Object</returns>
        ApiResponse<Object> PostMuseAgentCodeRepairV1(ActionCodeRepairRequest actionCodeRepairRequest);

        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> PostMuseAgentCodeRepairV1WithHttpInfo(ActionCodeRepairRequest actionCodeRepairRequest);
        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseAgentCodegen(CodeGenRequest codeGenRequest);

        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseAgentCodegenWithHttpInfo(CodeGenRequest codeGenRequest);
        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>Object</returns>
        ApiResponse<Object> PostMuseAgentCodegenV1(CodeGenRequest codeGenRequest);

        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> PostMuseAgentCodegenV1WithHttpInfo(CodeGenRequest codeGenRequest);
        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseChat(ChatRequest chatRequest);

        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseChatWithHttpInfo(ChatRequest chatRequest);
        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>Object</returns>
        ApiResponse<Object> PostMuseChatV1(ChatRequest chatRequest);

        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> PostMuseChatV1WithHttpInfo(ChatRequest chatRequest);
        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseCompletion(CompletionRequest completionRequest);

        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseCompletionWithHttpInfo(CompletionRequest completionRequest);
        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>Object</returns>
        ApiResponse<Object> PostMuseCompletionV1(CompletionRequest completionRequest);

        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> PostMuseCompletionV1WithHttpInfo(CompletionRequest completionRequest);
        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>Conversation</returns>
        [Obsolete]
        ApiResponse<Conversation> PostMuseConversation(CreateConversationRequest createConversationRequest);

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>ApiResponse of Conversation</returns>
        [Obsolete]
        ApiResponse<Conversation> PostMuseConversationWithHttpInfo(CreateConversationRequest createConversationRequest);
        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>Conversation</returns>
        ApiResponse<Conversation> PostMuseConversationV1(CreateConversationRequest createConversationRequest);

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>ApiResponse of Conversation</returns>
        ApiResponse<Conversation> PostMuseConversationV1WithHttpInfo(CreateConversationRequest createConversationRequest);
        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> PostMuseFeedback(Feedback feedback);

        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        ApiResponse<ErrorResponse> PostMuseFeedbackWithHttpInfo(Feedback feedback);
        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ErrorResponse</returns>
        ApiResponse<ErrorResponse> PostMuseFeedbackV1(Feedback feedback);

        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        ApiResponse<ErrorResponse> PostMuseFeedbackV1WithHttpInfo(Feedback feedback);
        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ResponsePostMuseInspiration</returns>
        [Obsolete]
        ApiResponse<ResponsePostMuseInspiration> PostMuseInspiration(Inspiration inspiration);

        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ApiResponse of ResponsePostMuseInspiration</returns>
        [Obsolete]
        ApiResponse<ResponsePostMuseInspiration> PostMuseInspirationWithHttpInfo(Inspiration inspiration);
        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ResponsePostMuseInspirationV1</returns>
        ApiResponse<ResponsePostMuseInspirationV1> PostMuseInspirationV1(Inspiration inspiration);

        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ApiResponse of ResponsePostMuseInspirationV1</returns>
        ApiResponse<ResponsePostMuseInspirationV1> PostMuseInspirationV1WithHttpInfo(Inspiration inspiration);
        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseOpt(OptRequest optRequest);

        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        ApiResponse<Object> PostMuseOptWithHttpInfo(OptRequest optRequest);
        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>Object</returns>
        ApiResponse<Object> PostMuseOptV1(OptRequest optRequest);

        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> PostMuseOptV1WithHttpInfo(OptRequest optRequest);
        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>SmartContextResponse</returns>
        [Obsolete]
        ApiResponse<SmartContextResponse> PostSmartContext(SmartContextRequest smartContextRequest);

        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>ApiResponse of SmartContextResponse</returns>
        [Obsolete]
        ApiResponse<SmartContextResponse> PostSmartContextWithHttpInfo(SmartContextRequest smartContextRequest);
        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>SmartContextResponse</returns>
        ApiResponse<SmartContextResponse> PostSmartContextV1(SmartContextRequest smartContextRequest);

        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>ApiResponse of SmartContextResponse</returns>
        ApiResponse<SmartContextResponse> PostSmartContextV1WithHttpInfo(SmartContextRequest smartContextRequest);
        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ResponsePutMuseInspirationUsingInspirationId</returns>
        [Obsolete]
        ApiResponse<ResponsePutMuseInspirationUsingInspirationId> PutMuseInspirationUsingInspirationId(string inspirationId, UpdateInspirationRequest updateInspirationRequest);

        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ApiResponse of ResponsePutMuseInspirationUsingInspirationId</returns>
        [Obsolete]
        ApiResponse<ResponsePutMuseInspirationUsingInspirationId> PutMuseInspirationUsingInspirationIdWithHttpInfo(string inspirationId, UpdateInspirationRequest updateInspirationRequest);
        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ResponsePutMuseInspirationUsingInspirationIdV1</returns>
        ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> PutMuseInspirationUsingInspirationIdV1(string inspirationId, UpdateInspirationRequest updateInspirationRequest);

        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ApiResponse of ResponsePutMuseInspirationUsingInspirationIdV1</returns>
        ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> PutMuseInspirationUsingInspirationIdV1WithHttpInfo(string inspirationId, UpdateInspirationRequest updateInspirationRequest);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    internal interface IMuseChatBackendApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdAsync(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfoAsync(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1Async(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Conversation Fragment
        /// </summary>
        /// <remarks>
        /// Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfoAsync(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdWithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdV1Async(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Conversation
        /// </summary>
        /// <remarks>
        /// Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdV1WithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsAsync(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsWithHttpInfoAsync(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsV1Async(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Conversations By Tags
        /// </summary>
        /// <remarks>
        /// Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsV1WithHttpInfoAsync(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Health
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetHealthAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Health
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetHealthWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Healthz
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetHealthzAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Healthz
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetHealthzWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of List&lt;ConversationInfo&gt;</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<List<ConversationInfo>>> GetMuseConversationAsync(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (List&lt;ConversationInfo&gt;)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<List<ConversationInfo>>> GetMuseConversationWithHttpInfoAsync(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseConversationUsingConversationId</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseConversationUsingConversationId>> GetMuseConversationUsingConversationIdAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseConversationUsingConversationId)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseConversationUsingConversationId>> GetMuseConversationUsingConversationIdWithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseConversationUsingConversationIdV1</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseConversationUsingConversationIdV1>> GetMuseConversationUsingConversationIdV1Async(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Conversation
        /// </summary>
        /// <remarks>
        /// Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseConversationUsingConversationIdV1)</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseConversationUsingConversationIdV1>> GetMuseConversationUsingConversationIdV1WithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of List&lt;ConversationInfo&gt;</returns>
        System.Threading.Tasks.Task<ApiResponse<List<ConversationInfo>>> GetMuseConversationV1Async(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Conversations
        /// </summary>
        /// <remarks>
        /// Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (List&lt;ConversationInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<ConversationInfo>>> GetMuseConversationV1WithHttpInfoAsync(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseInspiration</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseInspiration>> GetMuseInspirationAsync(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseInspiration)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseInspiration>> GetMuseInspirationWithHttpInfoAsync(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseInspirationV1</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseInspirationV1>> GetMuseInspirationV1Async(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Inspirations
        /// </summary>
        /// <remarks>
        /// Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseInspirationV1)</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponseGetMuseInspirationV1>> GetMuseInspirationV1WithHttpInfoAsync(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Dictionary&lt;string, OptDecision&gt;</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Dictionary&lt;string, OptDecision&gt;)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Dictionary&lt;string, OptDecision&gt;</returns>
        System.Threading.Tasks.Task<ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptV1Async(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Opt
        /// </summary>
        /// <remarks>
        /// Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Dictionary&lt;string, OptDecision&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptV1WithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of string</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<string>> GetMuseTopicUsingConversationIdAsync(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (string)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<string>> GetMuseTopicUsingConversationIdWithHttpInfoAsync(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of string</returns>
        System.Threading.Tasks.Task<ApiResponse<string>> GetMuseTopicUsingConversationIdV1Async(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>
        /// Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (string)</returns>
        System.Threading.Tasks.Task<ApiResponse<string>> GetMuseTopicUsingConversationIdV1WithHttpInfoAsync(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Health Head
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> HeadHealthAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Health Head
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> HeadHealthWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdAsync(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfoAsync(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1Async(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Patch Conversation Fragment Preference
        /// </summary>
        /// <remarks>
        /// Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfoAsync(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdAsync(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdWithHttpInfoAsync(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdV1Async(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Patch Conversation
        /// </summary>
        /// <remarks>
        /// Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdV1WithHttpInfoAsync(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentActionAsync(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentActionWithHttpInfoAsync(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentActionV1Async(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Action
        /// </summary>
        /// <remarks>
        /// Agent action route for performing actions in the editor.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentActionV1WithHttpInfoAsync(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodeRepairAsync(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodeRepairWithHttpInfoAsync(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodeRepairV1Async(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Action Code Repair
        /// </summary>
        /// <remarks>
        /// Agent action code repairing route for repairing generated csharp scripts.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodeRepairV1WithHttpInfoAsync(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodegenAsync(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodegenWithHttpInfoAsync(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodegenV1Async(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Codegen
        /// </summary>
        /// <remarks>
        /// POC of CodeGen route.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseAgentCodegenV1WithHttpInfoAsync(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseChatAsync(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseChatWithHttpInfoAsync(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseChatV1Async(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Chat
        /// </summary>
        /// <remarks>
        /// Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseChatV1WithHttpInfoAsync(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseCompletionAsync(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseCompletionWithHttpInfoAsync(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseCompletionV1Async(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Completion
        /// </summary>
        /// <remarks>
        /// Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseCompletionV1WithHttpInfoAsync(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Conversation</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Conversation>> PostMuseConversationAsync(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Conversation)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Conversation>> PostMuseConversationWithHttpInfoAsync(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Conversation</returns>
        System.Threading.Tasks.Task<ApiResponse<Conversation>> PostMuseConversationV1Async(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Conversation)</returns>
        System.Threading.Tasks.Task<ApiResponse<Conversation>> PostMuseConversationV1WithHttpInfoAsync(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PostMuseFeedbackAsync(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PostMuseFeedbackWithHttpInfoAsync(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PostMuseFeedbackV1Async(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<ErrorResponse>> PostMuseFeedbackV1WithHttpInfoAsync(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePostMuseInspiration</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponsePostMuseInspiration>> PostMuseInspirationAsync(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePostMuseInspiration)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponsePostMuseInspiration>> PostMuseInspirationWithHttpInfoAsync(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePostMuseInspirationV1</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponsePostMuseInspirationV1>> PostMuseInspirationV1Async(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create Inspiration
        /// </summary>
        /// <remarks>
        /// Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePostMuseInspirationV1)</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponsePostMuseInspirationV1>> PostMuseInspirationV1WithHttpInfoAsync(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseOptAsync(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseOptWithHttpInfoAsync(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseOptV1Async(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Opt
        /// </summary>
        /// <remarks>
        /// Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostMuseOptV1WithHttpInfoAsync(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of SmartContextResponse</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<SmartContextResponse>> PostSmartContextAsync(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (SmartContextResponse)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<SmartContextResponse>> PostSmartContextWithHttpInfoAsync(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of SmartContextResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<SmartContextResponse>> PostSmartContextV1Async(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Smart Context
        /// </summary>
        /// <remarks>
        /// Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (SmartContextResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<SmartContextResponse>> PostSmartContextV1WithHttpInfoAsync(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePutMuseInspirationUsingInspirationId</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponsePutMuseInspirationUsingInspirationId>> PutMuseInspirationUsingInspirationIdAsync(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePutMuseInspirationUsingInspirationId)</returns>
        [Obsolete]
        System.Threading.Tasks.Task<ApiResponse<ResponsePutMuseInspirationUsingInspirationId>> PutMuseInspirationUsingInspirationIdWithHttpInfoAsync(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePutMuseInspirationUsingInspirationIdV1</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1>> PutMuseInspirationUsingInspirationIdV1Async(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update Inspiration
        /// </summary>
        /// <remarks>
        /// Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </remarks>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePutMuseInspirationUsingInspirationIdV1)</returns>
        System.Threading.Tasks.Task<ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1>> PutMuseInspirationUsingInspirationIdV1WithHttpInfoAsync(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    internal interface IMuseChatBackendApi : IMuseChatBackendApiSync, IMuseChatBackendApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    internal partial class MuseChatBackendApi : IDisposable, IMuseChatBackendApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MuseChatBackendApi"/> class.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <returns></returns>
        public MuseChatBackendApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MuseChatBackendApi"/> class.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <param name="basePath">The target service's base path in URL format.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public MuseChatBackendApi(string basePath)
        {
            this.Configuration = Unity.Muse.Chat.BackendApi.Client.Configuration.MergeConfigurations(
                Unity.Muse.Chat.BackendApi.Client.GlobalConfiguration.Instance,
                new Unity.Muse.Chat.BackendApi.Client.Configuration { BasePath = basePath }
            );
            this.ApiClient = new Unity.Muse.Chat.BackendApi.Client.ApiClient(this.Configuration.BasePath);
            this.Client =  this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MuseChatBackendApi"/> class using Configuration object.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <param name="configuration">An instance of Configuration.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public MuseChatBackendApi(Unity.Muse.Chat.BackendApi.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = Unity.Muse.Chat.BackendApi.Client.Configuration.MergeConfigurations(
                Unity.Muse.Chat.BackendApi.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.ApiClient = new Unity.Muse.Chat.BackendApi.Client.ApiClient(this.Configuration.BasePath);
            this.Client = this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MuseChatBackendApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MuseChatBackendApi(Unity.Muse.Chat.BackendApi.Client.ISynchronousClient client, Unity.Muse.Chat.BackendApi.Client.IAsynchronousClient asyncClient, Unity.Muse.Chat.BackendApi.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Disposes resources if they were created by us
        /// </summary>
        public void Dispose()
        {
            this.ApiClient?.Dispose();
        }

        /// <summary>
        /// Holds the ApiClient if created
        /// </summary>
        public Unity.Muse.Chat.BackendApi.Client.ApiClient ApiClient { get; set; } = null;

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public Unity.Muse.Chat.BackendApi.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public Unity.Muse.Chat.BackendApi.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Unity.Muse.Chat.BackendApi.Client.IReadableConfiguration Configuration { get; set; }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        public ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentId(string conversationId, string fragmentId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfo(conversationId, fragmentId);
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfo(string conversationId, string fragmentId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentId");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<ErrorResponse>("/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdAsync(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfoAsync(conversationId, fragmentId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfoAsync(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentId");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.DeleteAsync<ErrorResponse>("/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ErrorResponse</returns>
        public ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1(string conversationId, string fragmentId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfo(conversationId, fragmentId);
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfo(string conversationId, string fragmentId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<ErrorResponse>("/v1/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1Async(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfoAsync(conversationId, fragmentId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Fragment Delete conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfoAsync(string conversationId, string fragmentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.DeleteAsync<ErrorResponse>("/v1/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        public ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationId(string conversationId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = DeleteMuseConversationUsingConversationIdWithHttpInfo(conversationId);
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationIdWithHttpInfo(string conversationId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationUsingConversationId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<ErrorResponse>("/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = DeleteMuseConversationUsingConversationIdWithHttpInfoAsync(conversationId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdWithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationUsingConversationId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.DeleteAsync<ErrorResponse>("/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ErrorResponse</returns>
        public ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationIdV1(string conversationId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = DeleteMuseConversationUsingConversationIdV1WithHttpInfo(conversationId);
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> DeleteMuseConversationUsingConversationIdV1WithHttpInfo(string conversationId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationUsingConversationIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<ErrorResponse>("/v1/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdV1Async(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = DeleteMuseConversationUsingConversationIdV1WithHttpInfoAsync(conversationId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversation Delete conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationUsingConversationIdV1WithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->DeleteMuseConversationUsingConversationIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.DeleteAsync<ErrorResponse>("/v1/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        public ApiResponse<ErrorResponse> DeleteMuseConversationsByTags(List<string> tags = default(List<string>))
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = DeleteMuseConversationsByTagsWithHttpInfo(tags);
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> DeleteMuseConversationsByTagsWithHttpInfo(List<string> tags = default(List<string>))
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("multi", "tags", tags));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<ErrorResponse>("/muse/conversations/by-tags", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsAsync(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = DeleteMuseConversationsByTagsWithHttpInfoAsync(tags, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsWithHttpInfoAsync(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("multi", "tags", tags));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.DeleteAsync<ErrorResponse>("/muse/conversations/by-tags", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ErrorResponse</returns>
        public ApiResponse<ErrorResponse> DeleteMuseConversationsByTagsV1(List<string> tags = default(List<string>))
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = DeleteMuseConversationsByTagsV1WithHttpInfo(tags);
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> DeleteMuseConversationsByTagsV1WithHttpInfo(List<string> tags = default(List<string>))
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("multi", "tags", tags));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<ErrorResponse>("/v1/muse/conversations/by-tags", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsV1Async(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = DeleteMuseConversationsByTagsV1WithHttpInfoAsync(tags, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Delete Conversations By Tags Delete conversations by tags.  Args:     request (Request): FastAPI request object.     tags (list[str])): list of tags.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags">List of tags to delete conversations by. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> DeleteMuseConversationsByTagsV1WithHttpInfoAsync(List<string> tags = default(List<string>), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("multi", "tags", tags));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.DeleteAsync<ErrorResponse>("/v1/muse/conversations/by-tags", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Health
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Object</returns>
        public ApiResponse<Object> GetHealth()
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = GetHealthWithHttpInfo();
            return localVarResponse;
        }

        /// <summary>
        /// Health
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> GetHealthWithHttpInfo()
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);



            // make the HTTP request
            var localVarResponse = this.Client.Get<Object>("/health", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Health
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> GetHealthAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetHealthWithHttpInfoAsync(cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Health
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> GetHealthWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);



            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<Object>("/health", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Healthz
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Object</returns>
        public ApiResponse<Object> GetHealthz()
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = GetHealthzWithHttpInfo();
            return localVarResponse;
        }

        /// <summary>
        /// Healthz
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> GetHealthzWithHttpInfo()
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);



            // make the HTTP request
            var localVarResponse = this.Client.Get<Object>("/healthz", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Healthz
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> GetHealthzAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetHealthzWithHttpInfoAsync(cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Healthz
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> GetHealthzWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);



            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<Object>("/healthz", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>List&lt;ConversationInfo&gt;</returns>
        [Obsolete]
        public ApiResponse<List<ConversationInfo>> GetMuseConversation(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> localVarResponse = GetMuseConversationWithHttpInfo(tags, skipProjectTag, limit, skip);
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of List&lt;ConversationInfo&gt;</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> GetMuseConversationWithHttpInfo(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "tags", tags));
            }
            if (skipProjectTag != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip_project_tag", skipProjectTag));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<List<ConversationInfo>>("/muse/conversation", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of List&lt;ConversationInfo&gt;</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>>> GetMuseConversationAsync(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseConversationWithHttpInfoAsync(tags, skipProjectTag, limit, skip, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (List&lt;ConversationInfo&gt;)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>>> GetMuseConversationWithHttpInfoAsync(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "tags", tags));
            }
            if (skipProjectTag != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip_project_tag", skipProjectTag));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<List<ConversationInfo>>("/muse/conversation", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ResponseGetMuseConversationUsingConversationId</returns>
        [Obsolete]
        public ApiResponse<ResponseGetMuseConversationUsingConversationId> GetMuseConversationUsingConversationId(string conversationId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationId> localVarResponse = GetMuseConversationUsingConversationIdWithHttpInfo(conversationId);
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ResponseGetMuseConversationUsingConversationId</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationId> GetMuseConversationUsingConversationIdWithHttpInfo(string conversationId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseConversationUsingConversationId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<ResponseGetMuseConversationUsingConversationId>("/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseConversationUsingConversationId</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationId>> GetMuseConversationUsingConversationIdAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseConversationUsingConversationIdWithHttpInfoAsync(conversationId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationId> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationId> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseConversationUsingConversationId)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationId>> GetMuseConversationUsingConversationIdWithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseConversationUsingConversationId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<ResponseGetMuseConversationUsingConversationId>("/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ResponseGetMuseConversationUsingConversationIdV1</returns>
        public ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> GetMuseConversationUsingConversationIdV1(string conversationId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> localVarResponse = GetMuseConversationUsingConversationIdV1WithHttpInfo(conversationId);
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <returns>ApiResponse of ResponseGetMuseConversationUsingConversationIdV1</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> GetMuseConversationUsingConversationIdV1WithHttpInfo(string conversationId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseConversationUsingConversationIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<ResponseGetMuseConversationUsingConversationIdV1>("/v1/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseConversationUsingConversationIdV1</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationIdV1>> GetMuseConversationUsingConversationIdV1Async(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseConversationUsingConversationIdV1WithHttpInfoAsync(conversationId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationIdV1> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversation Get conversation by conversation ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     ClientConversation | JSONResponse:         ClientConversation corresponding to ID if it exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseConversationUsingConversationIdV1)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseConversationUsingConversationIdV1>> GetMuseConversationUsingConversationIdV1WithHttpInfoAsync(string conversationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseConversationUsingConversationIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<ResponseGetMuseConversationUsingConversationIdV1>("/v1/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>List&lt;ConversationInfo&gt;</returns>
        public ApiResponse<List<ConversationInfo>> GetMuseConversationV1(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> localVarResponse = GetMuseConversationV1WithHttpInfo(tags, skipProjectTag, limit, skip);
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of List&lt;ConversationInfo&gt;</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> GetMuseConversationV1WithHttpInfo(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "tags", tags));
            }
            if (skipProjectTag != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip_project_tag", skipProjectTag));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<List<ConversationInfo>>("/v1/muse/conversation", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of List&lt;ConversationInfo&gt;</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>>> GetMuseConversationV1Async(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseConversationV1WithHttpInfoAsync(tags, skipProjectTag, limit, skip, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Conversations Get conversation summaries for user conversations.  Args:     request (Request): FastAPI request object.     user_info (UserInfo): User information extracted from bearer token.     tags (Optional[str], optional): Project ID to filter conversations by. Defaults to None.     skip_project_tag (bool, optional): Whether to skip conversations with a project tag.     limit (int, optional): Number of conversations to return. Defaults to 100.     skip (int, optional): Number of conversations to skip. Defaults to 0.  Returns:     list[ConversationInfo]: List of conversation summaries for user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tags"> (optional)</param>
        /// <param name="skipProjectTag"> (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (List&lt;ConversationInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<List<ConversationInfo>>> GetMuseConversationV1WithHttpInfoAsync(string tags = default(string), bool? skipProjectTag = default(bool?), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (tags != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "tags", tags));
            }
            if (skipProjectTag != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip_project_tag", skipProjectTag));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<List<ConversationInfo>>("/v1/muse/conversation", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ResponseGetMuseInspiration</returns>
        [Obsolete]
        public ApiResponse<ResponseGetMuseInspiration> GetMuseInspiration(string mode = default(string), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspiration> localVarResponse = GetMuseInspirationWithHttpInfo(mode, limit, skip);
            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of ResponseGetMuseInspiration</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspiration> GetMuseInspirationWithHttpInfo(string mode = default(string), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (mode != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "mode", mode));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<ResponseGetMuseInspiration>("/muse/inspiration/", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseInspiration</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspiration>> GetMuseInspirationAsync(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseInspirationWithHttpInfoAsync(mode, limit, skip, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspiration> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspiration> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseInspiration)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspiration>> GetMuseInspirationWithHttpInfoAsync(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (mode != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "mode", mode));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<ResponseGetMuseInspiration>("/muse/inspiration/", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ResponseGetMuseInspirationV1</returns>
        public ApiResponse<ResponseGetMuseInspirationV1> GetMuseInspirationV1(string mode = default(string), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspirationV1> localVarResponse = GetMuseInspirationV1WithHttpInfo(mode, limit, skip);
            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <returns>ApiResponse of ResponseGetMuseInspirationV1</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspirationV1> GetMuseInspirationV1WithHttpInfo(string mode = default(string), int? limit = default(int?), int? skip = default(int?))
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (mode != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "mode", mode));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<ResponseGetMuseInspirationV1>("/v1/muse/inspiration/", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponseGetMuseInspirationV1</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspirationV1>> GetMuseInspirationV1Async(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseInspirationV1WithHttpInfoAsync(mode, limit, skip, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspirationV1> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspirationV1> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Inspirations Get inspirations from the database.  Args:     request: FastAPI request object.     mode: Filter inspirations by mode.     limit: Number of inspirations to return.     skip: Number of inspirations to skip.  Returns: List of inspirations or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="mode">Filter inspirations by mode (optional)</param>
        /// <param name="limit"> (optional, default to 100)</param>
        /// <param name="skip"> (optional, default to 0)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponseGetMuseInspirationV1)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponseGetMuseInspirationV1>> GetMuseInspirationV1WithHttpInfoAsync(string mode = default(string), int? limit = default(int?), int? skip = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (mode != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "mode", mode));
            }
            if (limit != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "limit", limit));
            }
            if (skip != null)
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "skip", skip));
            }

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<ResponseGetMuseInspirationV1>("/v1/muse/inspiration/", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Dictionary&lt;string, OptDecision&gt;</returns>
        [Obsolete]
        public ApiResponse<Dictionary<string, OptDecision>> GetMuseOpt()
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> localVarResponse = GetMuseOptWithHttpInfo();
            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Dictionary&lt;string, OptDecision&gt;</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> GetMuseOptWithHttpInfo()
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);


            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<Dictionary<string, OptDecision>>("/muse/opt", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Dictionary&lt;string, OptDecision&gt;</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseOptWithHttpInfoAsync(cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Dictionary&lt;string, OptDecision&gt;)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);


            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<Dictionary<string, OptDecision>>("/muse/opt", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Dictionary&lt;string, OptDecision&gt;</returns>
        public ApiResponse<Dictionary<string, OptDecision>> GetMuseOptV1()
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> localVarResponse = GetMuseOptV1WithHttpInfo();
            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Dictionary&lt;string, OptDecision&gt;</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> GetMuseOptV1WithHttpInfo()
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);


            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<Dictionary<string, OptDecision>>("/v1/muse/opt", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Dictionary&lt;string, OptDecision&gt;</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptV1Async(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseOptV1WithHttpInfoAsync(cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Opt Get the current opt status of the requesting user.  Args:     request (Request): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).  Returns:     dict[OptType, OptDecision]: Opt status of the user.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Dictionary&lt;string, OptDecision&gt;)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Dictionary<string, OptDecision>>> GetMuseOptV1WithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);


            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<Dictionary<string, OptDecision>>("/v1/muse/opt", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>string</returns>
        [Obsolete]
        public ApiResponse<string> GetMuseTopicUsingConversationId(string conversationId, string organizationId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> localVarResponse = GetMuseTopicUsingConversationIdWithHttpInfo(conversationId, organizationId);
            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>ApiResponse of string</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> GetMuseTopicUsingConversationIdWithHttpInfo(string conversationId, string organizationId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationId");

            // verify the required parameter 'organizationId' is set
            if (organizationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'organizationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "organization_id", organizationId));

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<string>("/muse/topic/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of string</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<string>> GetMuseTopicUsingConversationIdAsync(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseTopicUsingConversationIdWithHttpInfoAsync(conversationId, organizationId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (string)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<string>> GetMuseTopicUsingConversationIdWithHttpInfoAsync(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationId");

            // verify the required parameter 'organizationId' is set
            if (organizationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'organizationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "organization_id", organizationId));

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<string>("/muse/topic/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>string</returns>
        public ApiResponse<string> GetMuseTopicUsingConversationIdV1(string conversationId, string organizationId)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> localVarResponse = GetMuseTopicUsingConversationIdV1WithHttpInfo(conversationId, organizationId);
            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <returns>ApiResponse of string</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> GetMuseTopicUsingConversationIdV1WithHttpInfo(string conversationId, string organizationId)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationIdV1");

            // verify the required parameter 'organizationId' is set
            if (organizationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'organizationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "organization_id", organizationId));

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<string>("/v1/muse/topic/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of string</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<string>> GetMuseTopicUsingConversationIdV1Async(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = GetMuseTopicUsingConversationIdV1WithHttpInfoAsync(conversationId, organizationId, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<string> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Get Topic Get topic title for conversation.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     organization_id (str): Organization ID.     user_info (UserInfo): User information extracted from bearer token.  Returns:     str | JSONResponse:         Plain-text topic if conversation exists, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (string)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<string>> GetMuseTopicUsingConversationIdV1WithHttpInfoAsync(string conversationId, string organizationId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationIdV1");

            // verify the required parameter 'organizationId' is set
            if (organizationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'organizationId' when calling MuseChatBackendApi->GetMuseTopicUsingConversationIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "organization_id", organizationId));

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.GetAsync<string>("/v1/muse/topic/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Health Head
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Object</returns>
        public ApiResponse<Object> HeadHealth()
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = HeadHealthWithHttpInfo();
            return localVarResponse;
        }

        /// <summary>
        /// Health Head
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> HeadHealthWithHttpInfo()
        {
            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);



            // make the HTTP request
            var localVarResponse = this.Client.Head<Object>("/health", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Health Head
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> HeadHealthAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = HeadHealthWithHttpInfoAsync(cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Health Head
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> HeadHealthWithHttpInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);



            // make the HTTP request

            var task = this.AsynchronousClient.HeadAsync<Object>("/health", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        public ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentId(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = PatchMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfo(conversationId, fragmentId, conversationFragmentPatch);
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfo(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentId");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentId");

            // verify the required parameter 'conversationFragmentPatch' is set
            if (conversationFragmentPatch == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationFragmentPatch' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter
            localVarRequestOptions.Data = conversationFragmentPatch;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Patch<ErrorResponse>("/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdAsync(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PatchMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfoAsync(conversationId, fragmentId, conversationFragmentPatch, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdWithHttpInfoAsync(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentId");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentId");

            // verify the required parameter 'conversationFragmentPatch' is set
            if (conversationFragmentPatch == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationFragmentPatch' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter
            localVarRequestOptions.Data = conversationFragmentPatch;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PatchAsync<ErrorResponse>("/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ErrorResponse</returns>
        public ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfo(conversationId, fragmentId, conversationFragmentPatch);
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfo(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            // verify the required parameter 'conversationFragmentPatch' is set
            if (conversationFragmentPatch == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationFragmentPatch' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter
            localVarRequestOptions.Data = conversationFragmentPatch;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Patch<ErrorResponse>("/v1/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1Async(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfoAsync(conversationId, fragmentId, conversationFragmentPatch, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Fragment Preference Update conversation fragment by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     fragment_id (str): Conversation fragment ID.     body (ConversationPatchRequest): Patch request for changing conversation fragment.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="fragmentId"></param>
        /// <param name="conversationFragmentPatch"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1WithHttpInfoAsync(string conversationId, string fragmentId, ConversationFragmentPatch conversationFragmentPatch, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            // verify the required parameter 'fragmentId' is set
            if (fragmentId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'fragmentId' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1");

            // verify the required parameter 'conversationFragmentPatch' is set
            if (conversationFragmentPatch == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationFragmentPatch' when calling MuseChatBackendApi->PatchMuseConversationFragmentUsingConversationIdAndFragmentIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.PathParameters.Add("fragment_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(fragmentId)); // path parameter
            localVarRequestOptions.Data = conversationFragmentPatch;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PatchAsync<ErrorResponse>("/v1/muse/conversation/{conversation_id}/fragment/{fragment_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        public ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationId(string conversationId, ConversationPatchRequest conversationPatchRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = PatchMuseConversationUsingConversationIdWithHttpInfo(conversationId, conversationPatchRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationIdWithHttpInfo(string conversationId, ConversationPatchRequest conversationPatchRequest)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationId");

            // verify the required parameter 'conversationPatchRequest' is set
            if (conversationPatchRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationPatchRequest' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.Data = conversationPatchRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Patch<ErrorResponse>("/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdAsync(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PatchMuseConversationUsingConversationIdWithHttpInfoAsync(conversationId, conversationPatchRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdWithHttpInfoAsync(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationId");

            // verify the required parameter 'conversationPatchRequest' is set
            if (conversationPatchRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationPatchRequest' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.Data = conversationPatchRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PatchAsync<ErrorResponse>("/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ErrorResponse</returns>
        public ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationIdV1(string conversationId, ConversationPatchRequest conversationPatchRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = PatchMuseConversationUsingConversationIdV1WithHttpInfo(conversationId, conversationPatchRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> PatchMuseConversationUsingConversationIdV1WithHttpInfo(string conversationId, ConversationPatchRequest conversationPatchRequest)
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationIdV1");

            // verify the required parameter 'conversationPatchRequest' is set
            if (conversationPatchRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationPatchRequest' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.Data = conversationPatchRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Patch<ErrorResponse>("/v1/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdV1Async(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PatchMuseConversationUsingConversationIdV1WithHttpInfoAsync(conversationId, conversationPatchRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Patch Conversation Update conversation by ID.  Args:     request (Request): FastAPI request object.     conversation_id (str): Conversation ID.     body (ConversationPatchRequest): Patch request for changing conversation.     user_info (UserInfo): User information extracted from bearer token.  Returns:     None | JSONResponse: None if successful, otherwise ErrorResponse.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="conversationId"></param>
        /// <param name="conversationPatchRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PatchMuseConversationUsingConversationIdV1WithHttpInfoAsync(string conversationId, ConversationPatchRequest conversationPatchRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'conversationId' is set
            if (conversationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationId' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationIdV1");

            // verify the required parameter 'conversationPatchRequest' is set
            if (conversationPatchRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'conversationPatchRequest' when calling MuseChatBackendApi->PatchMuseConversationUsingConversationIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("conversation_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(conversationId)); // path parameter
            localVarRequestOptions.Data = conversationPatchRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PatchAsync<ErrorResponse>("/v1/muse/conversation/{conversation_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        public ApiResponse<Object> PostMuseAgentAction(ActionRequest actionRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseAgentActionWithHttpInfo(actionRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseAgentActionWithHttpInfo(ActionRequest actionRequest)
        {
            // verify the required parameter 'actionRequest' is set
            if (actionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionRequest' when calling MuseChatBackendApi->PostMuseAgentAction");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/muse/agent/action", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentActionAsync(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseAgentActionWithHttpInfoAsync(actionRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentActionWithHttpInfoAsync(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'actionRequest' is set
            if (actionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionRequest' when calling MuseChatBackendApi->PostMuseAgentAction");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/muse/agent/action", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>Object</returns>
        public ApiResponse<Object> PostMuseAgentActionV1(ActionRequest actionRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseAgentActionV1WithHttpInfo(actionRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseAgentActionV1WithHttpInfo(ActionRequest actionRequest)
        {
            // verify the required parameter 'actionRequest' is set
            if (actionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionRequest' when calling MuseChatBackendApi->PostMuseAgentActionV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/v1/muse/agent/action", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentActionV1Async(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseAgentActionV1WithHttpInfoAsync(actionRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Action Agent action route for performing actions in the editor.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentActionV1WithHttpInfoAsync(ActionRequest actionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'actionRequest' is set
            if (actionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionRequest' when calling MuseChatBackendApi->PostMuseAgentActionV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/v1/muse/agent/action", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        public ApiResponse<Object> PostMuseAgentCodeRepair(ActionCodeRepairRequest actionCodeRepairRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseAgentCodeRepairWithHttpInfo(actionCodeRepairRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseAgentCodeRepairWithHttpInfo(ActionCodeRepairRequest actionCodeRepairRequest)
        {
            // verify the required parameter 'actionCodeRepairRequest' is set
            if (actionCodeRepairRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionCodeRepairRequest' when calling MuseChatBackendApi->PostMuseAgentCodeRepair");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionCodeRepairRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/muse/agent/code_repair", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodeRepairAsync(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseAgentCodeRepairWithHttpInfoAsync(actionCodeRepairRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodeRepairWithHttpInfoAsync(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'actionCodeRepairRequest' is set
            if (actionCodeRepairRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionCodeRepairRequest' when calling MuseChatBackendApi->PostMuseAgentCodeRepair");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionCodeRepairRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/muse/agent/code_repair", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>Object</returns>
        public ApiResponse<Object> PostMuseAgentCodeRepairV1(ActionCodeRepairRequest actionCodeRepairRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseAgentCodeRepairV1WithHttpInfo(actionCodeRepairRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseAgentCodeRepairV1WithHttpInfo(ActionCodeRepairRequest actionCodeRepairRequest)
        {
            // verify the required parameter 'actionCodeRepairRequest' is set
            if (actionCodeRepairRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionCodeRepairRequest' when calling MuseChatBackendApi->PostMuseAgentCodeRepairV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionCodeRepairRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/v1/muse/agent/code_repair", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodeRepairV1Async(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseAgentCodeRepairV1WithHttpInfoAsync(actionCodeRepairRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Action Code Repair Agent action code repairing route for repairing generated csharp scripts.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="actionCodeRepairRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodeRepairV1WithHttpInfoAsync(ActionCodeRepairRequest actionCodeRepairRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'actionCodeRepairRequest' is set
            if (actionCodeRepairRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'actionCodeRepairRequest' when calling MuseChatBackendApi->PostMuseAgentCodeRepairV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = actionCodeRepairRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/v1/muse/agent/code_repair", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        public ApiResponse<Object> PostMuseAgentCodegen(CodeGenRequest codeGenRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseAgentCodegenWithHttpInfo(codeGenRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseAgentCodegenWithHttpInfo(CodeGenRequest codeGenRequest)
        {
            // verify the required parameter 'codeGenRequest' is set
            if (codeGenRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'codeGenRequest' when calling MuseChatBackendApi->PostMuseAgentCodegen");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = codeGenRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/muse/agent/codegen", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodegenAsync(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseAgentCodegenWithHttpInfoAsync(codeGenRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodegenWithHttpInfoAsync(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'codeGenRequest' is set
            if (codeGenRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'codeGenRequest' when calling MuseChatBackendApi->PostMuseAgentCodegen");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = codeGenRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/muse/agent/codegen", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>Object</returns>
        public ApiResponse<Object> PostMuseAgentCodegenV1(CodeGenRequest codeGenRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseAgentCodegenV1WithHttpInfo(codeGenRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseAgentCodegenV1WithHttpInfo(CodeGenRequest codeGenRequest)
        {
            // verify the required parameter 'codeGenRequest' is set
            if (codeGenRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'codeGenRequest' when calling MuseChatBackendApi->PostMuseAgentCodegenV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = codeGenRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/v1/muse/agent/codegen", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodegenV1Async(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseAgentCodegenV1WithHttpInfoAsync(codeGenRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Codegen POC of CodeGen route.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codeGenRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseAgentCodegenV1WithHttpInfoAsync(CodeGenRequest codeGenRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'codeGenRequest' is set
            if (codeGenRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'codeGenRequest' when calling MuseChatBackendApi->PostMuseAgentCodegenV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = codeGenRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/v1/muse/agent/codegen", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        public ApiResponse<Object> PostMuseChat(ChatRequest chatRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseChatWithHttpInfo(chatRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseChatWithHttpInfo(ChatRequest chatRequest)
        {
            // verify the required parameter 'chatRequest' is set
            if (chatRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'chatRequest' when calling MuseChatBackendApi->PostMuseChat");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = chatRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/muse/chat", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseChatAsync(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseChatWithHttpInfoAsync(chatRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseChatWithHttpInfoAsync(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'chatRequest' is set
            if (chatRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'chatRequest' when calling MuseChatBackendApi->PostMuseChat");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = chatRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/muse/chat", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>Object</returns>
        public ApiResponse<Object> PostMuseChatV1(ChatRequest chatRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseChatV1WithHttpInfo(chatRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseChatV1WithHttpInfo(ChatRequest chatRequest)
        {
            // verify the required parameter 'chatRequest' is set
            if (chatRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'chatRequest' when calling MuseChatBackendApi->PostMuseChatV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = chatRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/v1/muse/chat", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseChatV1Async(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseChatV1WithHttpInfoAsync(chatRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Chat Chat with Muse.  Args:     request (Request): FastAPI request object.     body (ChatRequest): Chat request body.     user_info (UserInfo): User information extracted from bearer token.     conversation (Conversation): Conversation to chat in.     classification_or_deny (ClassificationModel | UnsafeQueryResponse): Classification model or         unsafe query response.  Returns:     StreamingResponse | ChatResponse | JSONResponse:         Either streaming response, at-once chat response, or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseChatV1WithHttpInfoAsync(ChatRequest chatRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'chatRequest' is set
            if (chatRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'chatRequest' when calling MuseChatBackendApi->PostMuseChatV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = chatRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/v1/muse/chat", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        public ApiResponse<Object> PostMuseCompletion(CompletionRequest completionRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseCompletionWithHttpInfo(completionRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseCompletionWithHttpInfo(CompletionRequest completionRequest)
        {
            // verify the required parameter 'completionRequest' is set
            if (completionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'completionRequest' when calling MuseChatBackendApi->PostMuseCompletion");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = completionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/muse/completion", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseCompletionAsync(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseCompletionWithHttpInfoAsync(completionRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseCompletionWithHttpInfoAsync(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'completionRequest' is set
            if (completionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'completionRequest' when calling MuseChatBackendApi->PostMuseCompletion");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = completionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/muse/completion", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>Object</returns>
        public ApiResponse<Object> PostMuseCompletionV1(CompletionRequest completionRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseCompletionV1WithHttpInfo(completionRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseCompletionV1WithHttpInfo(CompletionRequest completionRequest)
        {
            // verify the required parameter 'completionRequest' is set
            if (completionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'completionRequest' when calling MuseChatBackendApi->PostMuseCompletionV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = completionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/v1/muse/completion", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseCompletionV1Async(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseCompletionV1WithHttpInfoAsync(completionRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Completion Handles completion requests for a conversational AI model and manages associated user conversations and analytics.  Args:     request (Request): The request object, which provides access to all request-specific data.     body (CompletionRequest): The request body containing data necessary for completion request.     user_info (UserInfo): User information extracted from bearer token.     background_tasks: BackgroundTasks: FastAPI background tasks object.  Returns:     Union[StreamingResponse, ChatResponse, JSONResponse]: Based on the &#x60;stream_response&#x60; flag in the request body,     this could be either a directly returned chat response, a streaming response,     or a JSON response containing the chat output.  Raises:     HTTPException: An error response with status code 500 in case of a failure during chat handling operations.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="completionRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseCompletionV1WithHttpInfoAsync(CompletionRequest completionRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'completionRequest' is set
            if (completionRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'completionRequest' when calling MuseChatBackendApi->PostMuseCompletionV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = completionRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/v1/muse/completion", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>Conversation</returns>
        [Obsolete]
        public ApiResponse<Conversation> PostMuseConversation(CreateConversationRequest createConversationRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> localVarResponse = PostMuseConversationWithHttpInfo(createConversationRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>ApiResponse of Conversation</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> PostMuseConversationWithHttpInfo(CreateConversationRequest createConversationRequest)
        {
            // verify the required parameter 'createConversationRequest' is set
            if (createConversationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'createConversationRequest' when calling MuseChatBackendApi->PostMuseConversation");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = createConversationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Conversation>("/muse/conversation", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Conversation</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation>> PostMuseConversationAsync(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseConversationWithHttpInfoAsync(createConversationRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Conversation)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation>> PostMuseConversationWithHttpInfoAsync(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'createConversationRequest' is set
            if (createConversationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'createConversationRequest' when calling MuseChatBackendApi->PostMuseConversation");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = createConversationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Conversation>("/muse/conversation", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>Conversation</returns>
        public ApiResponse<Conversation> PostMuseConversationV1(CreateConversationRequest createConversationRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> localVarResponse = PostMuseConversationV1WithHttpInfo(createConversationRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <returns>ApiResponse of Conversation</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> PostMuseConversationV1WithHttpInfo(CreateConversationRequest createConversationRequest)
        {
            // verify the required parameter 'createConversationRequest' is set
            if (createConversationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'createConversationRequest' when calling MuseChatBackendApi->PostMuseConversationV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = createConversationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Conversation>("/v1/muse/conversation", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Conversation</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation>> PostMuseConversationV1Async(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseConversationV1WithHttpInfoAsync(createConversationRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Create Conversation
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createConversationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Conversation)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Conversation>> PostMuseConversationV1WithHttpInfoAsync(CreateConversationRequest createConversationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'createConversationRequest' is set
            if (createConversationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'createConversationRequest' when calling MuseChatBackendApi->PostMuseConversationV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = createConversationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Conversation>("/v1/muse/conversation", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ErrorResponse</returns>
        [Obsolete]
        public ApiResponse<ErrorResponse> PostMuseFeedback(Feedback feedback)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = PostMuseFeedbackWithHttpInfo(feedback);
            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> PostMuseFeedbackWithHttpInfo(Feedback feedback)
        {
            // verify the required parameter 'feedback' is set
            if (feedback == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'feedback' when calling MuseChatBackendApi->PostMuseFeedback");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = feedback;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<ErrorResponse>("/muse/feedback", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PostMuseFeedbackAsync(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseFeedbackWithHttpInfoAsync(feedback, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PostMuseFeedbackWithHttpInfoAsync(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'feedback' is set
            if (feedback == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'feedback' when calling MuseChatBackendApi->PostMuseFeedback");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = feedback;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<ErrorResponse>("/muse/feedback", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ErrorResponse</returns>
        public ApiResponse<ErrorResponse> PostMuseFeedbackV1(Feedback feedback)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = PostMuseFeedbackV1WithHttpInfo(feedback);
            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <returns>ApiResponse of ErrorResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> PostMuseFeedbackV1WithHttpInfo(Feedback feedback)
        {
            // verify the required parameter 'feedback' is set
            if (feedback == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'feedback' when calling MuseChatBackendApi->PostMuseFeedbackV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = feedback;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<ErrorResponse>("/v1/muse/feedback", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ErrorResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PostMuseFeedbackV1Async(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseFeedbackV1WithHttpInfoAsync(feedback, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback.  Args:     request (Request): FastAPI request object.     body (Feedback): Feedback request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     Optional[JSONResponse]: Nothing if successful, otherwise JSONResponse with error.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedback"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ErrorResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ErrorResponse>> PostMuseFeedbackV1WithHttpInfoAsync(Feedback feedback, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'feedback' is set
            if (feedback == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'feedback' when calling MuseChatBackendApi->PostMuseFeedbackV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = feedback;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<ErrorResponse>("/v1/muse/feedback", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ResponsePostMuseInspiration</returns>
        [Obsolete]
        public ApiResponse<ResponsePostMuseInspiration> PostMuseInspiration(Inspiration inspiration)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspiration> localVarResponse = PostMuseInspirationWithHttpInfo(inspiration);
            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ApiResponse of ResponsePostMuseInspiration</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspiration> PostMuseInspirationWithHttpInfo(Inspiration inspiration)
        {
            // verify the required parameter 'inspiration' is set
            if (inspiration == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspiration' when calling MuseChatBackendApi->PostMuseInspiration");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = inspiration;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<ResponsePostMuseInspiration>("/muse/inspiration/", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePostMuseInspiration</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspiration>> PostMuseInspirationAsync(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseInspirationWithHttpInfoAsync(inspiration, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspiration> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspiration> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePostMuseInspiration)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspiration>> PostMuseInspirationWithHttpInfoAsync(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'inspiration' is set
            if (inspiration == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspiration' when calling MuseChatBackendApi->PostMuseInspiration");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = inspiration;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<ResponsePostMuseInspiration>("/muse/inspiration/", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ResponsePostMuseInspirationV1</returns>
        public ApiResponse<ResponsePostMuseInspirationV1> PostMuseInspirationV1(Inspiration inspiration)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspirationV1> localVarResponse = PostMuseInspirationV1WithHttpInfo(inspiration);
            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <returns>ApiResponse of ResponsePostMuseInspirationV1</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspirationV1> PostMuseInspirationV1WithHttpInfo(Inspiration inspiration)
        {
            // verify the required parameter 'inspiration' is set
            if (inspiration == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspiration' when calling MuseChatBackendApi->PostMuseInspirationV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = inspiration;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<ResponsePostMuseInspirationV1>("/v1/muse/inspiration/", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePostMuseInspirationV1</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspirationV1>> PostMuseInspirationV1Async(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseInspirationV1WithHttpInfoAsync(inspiration, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspirationV1> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspirationV1> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Create Inspiration Create a new inspiration in the database.  Args:     request: FastAPI request object.     body: Inspiration object to create.  Returns: Created inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspiration"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePostMuseInspirationV1)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePostMuseInspirationV1>> PostMuseInspirationV1WithHttpInfoAsync(Inspiration inspiration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'inspiration' is set
            if (inspiration == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspiration' when calling MuseChatBackendApi->PostMuseInspirationV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = inspiration;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<ResponsePostMuseInspirationV1>("/v1/muse/inspiration/", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>Object</returns>
        [Obsolete]
        public ApiResponse<Object> PostMuseOpt(OptRequest optRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseOptWithHttpInfo(optRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseOptWithHttpInfo(OptRequest optRequest)
        {
            // verify the required parameter 'optRequest' is set
            if (optRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'optRequest' when calling MuseChatBackendApi->PostMuseOpt");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = optRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/muse/opt", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseOptAsync(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseOptWithHttpInfoAsync(optRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseOptWithHttpInfoAsync(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'optRequest' is set
            if (optRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'optRequest' when calling MuseChatBackendApi->PostMuseOpt");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = optRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/muse/opt", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>Object</returns>
        public ApiResponse<Object> PostMuseOptV1(OptRequest optRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = PostMuseOptV1WithHttpInfo(optRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <returns>ApiResponse of Object</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> PostMuseOptV1WithHttpInfo(OptRequest optRequest)
        {
            // verify the required parameter 'optRequest' is set
            if (optRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'optRequest' when calling MuseChatBackendApi->PostMuseOptV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = optRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/v1/muse/opt", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseOptV1Async(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostMuseOptV1WithHttpInfoAsync(optRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Opt Opt in or out of model training.  Notes:     This is ideally a temporary solution. :)  Args:     request (Request): _description_     body (OptRequest): _description_     user_info (UserInfo, optional): _description_. Defaults to Depends(extract_user_info).
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="optRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<Object>> PostMuseOptV1WithHttpInfoAsync(OptRequest optRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'optRequest' is set
            if (optRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'optRequest' when calling MuseChatBackendApi->PostMuseOptV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = optRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/v1/muse/opt", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>SmartContextResponse</returns>
        [Obsolete]
        public ApiResponse<SmartContextResponse> PostSmartContext(SmartContextRequest smartContextRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> localVarResponse = PostSmartContextWithHttpInfo(smartContextRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>ApiResponse of SmartContextResponse</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> PostSmartContextWithHttpInfo(SmartContextRequest smartContextRequest)
        {
            // verify the required parameter 'smartContextRequest' is set
            if (smartContextRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'smartContextRequest' when calling MuseChatBackendApi->PostSmartContext");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = smartContextRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<SmartContextResponse>("/smart-context", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of SmartContextResponse</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse>> PostSmartContextAsync(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostSmartContextWithHttpInfoAsync(smartContextRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (SmartContextResponse)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse>> PostSmartContextWithHttpInfoAsync(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'smartContextRequest' is set
            if (smartContextRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'smartContextRequest' when calling MuseChatBackendApi->PostSmartContext");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = smartContextRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<SmartContextResponse>("/smart-context", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>SmartContextResponse</returns>
        public ApiResponse<SmartContextResponse> PostSmartContextV1(SmartContextRequest smartContextRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> localVarResponse = PostSmartContextV1WithHttpInfo(smartContextRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <returns>ApiResponse of SmartContextResponse</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> PostSmartContextV1WithHttpInfo(SmartContextRequest smartContextRequest)
        {
            // verify the required parameter 'smartContextRequest' is set
            if (smartContextRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'smartContextRequest' when calling MuseChatBackendApi->PostSmartContextV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = smartContextRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<SmartContextResponse>("/v1/smart-context", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of SmartContextResponse</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse>> PostSmartContextV1Async(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PostSmartContextV1WithHttpInfoAsync(smartContextRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Smart Context Handle smart context requests.  Args:     request (Request): FastAPI request object.     body (SmartContextRequest): Smart context request body.     user_info (UserInfo): User information extracted from bearer token.  Returns:     SmartContextResponse | JSONResponse:         Either smart context response or JSON error message.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="smartContextRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (SmartContextResponse)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<SmartContextResponse>> PostSmartContextV1WithHttpInfoAsync(SmartContextRequest smartContextRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'smartContextRequest' is set
            if (smartContextRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'smartContextRequest' when calling MuseChatBackendApi->PostSmartContextV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = smartContextRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<SmartContextResponse>("/v1/smart-context", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ResponsePutMuseInspirationUsingInspirationId</returns>
        [Obsolete]
        public ApiResponse<ResponsePutMuseInspirationUsingInspirationId> PutMuseInspirationUsingInspirationId(string inspirationId, UpdateInspirationRequest updateInspirationRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationId> localVarResponse = PutMuseInspirationUsingInspirationIdWithHttpInfo(inspirationId, updateInspirationRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ApiResponse of ResponsePutMuseInspirationUsingInspirationId</returns>
        [Obsolete]
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationId> PutMuseInspirationUsingInspirationIdWithHttpInfo(string inspirationId, UpdateInspirationRequest updateInspirationRequest)
        {
            // verify the required parameter 'inspirationId' is set
            if (inspirationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspirationId' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationId");

            // verify the required parameter 'updateInspirationRequest' is set
            if (updateInspirationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'updateInspirationRequest' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationId");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("inspiration_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(inspirationId)); // path parameter
            localVarRequestOptions.Data = updateInspirationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Put<ResponsePutMuseInspirationUsingInspirationId>("/muse/inspiration/{inspiration_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePutMuseInspirationUsingInspirationId</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationId>> PutMuseInspirationUsingInspirationIdAsync(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PutMuseInspirationUsingInspirationIdWithHttpInfoAsync(inspirationId, updateInspirationRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationId> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationId> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePutMuseInspirationUsingInspirationId)</returns>
        [Obsolete]
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationId>> PutMuseInspirationUsingInspirationIdWithHttpInfoAsync(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'inspirationId' is set
            if (inspirationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspirationId' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationId");

            // verify the required parameter 'updateInspirationRequest' is set
            if (updateInspirationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'updateInspirationRequest' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationId");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("inspiration_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(inspirationId)); // path parameter
            localVarRequestOptions.Data = updateInspirationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PutAsync<ResponsePutMuseInspirationUsingInspirationId>("/muse/inspiration/{inspiration_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ResponsePutMuseInspirationUsingInspirationIdV1</returns>
        public ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> PutMuseInspirationUsingInspirationIdV1(string inspirationId, UpdateInspirationRequest updateInspirationRequest)
        {
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> localVarResponse = PutMuseInspirationUsingInspirationIdV1WithHttpInfo(inspirationId, updateInspirationRequest);
            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <returns>ApiResponse of ResponsePutMuseInspirationUsingInspirationIdV1</returns>
        public Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> PutMuseInspirationUsingInspirationIdV1WithHttpInfo(string inspirationId, UpdateInspirationRequest updateInspirationRequest)
        {
            // verify the required parameter 'inspirationId' is set
            if (inspirationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspirationId' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationIdV1");

            // verify the required parameter 'updateInspirationRequest' is set
            if (updateInspirationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'updateInspirationRequest' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationIdV1");

            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("inspiration_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(inspirationId)); // path parameter
            localVarRequestOptions.Data = updateInspirationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Put<ResponsePutMuseInspirationUsingInspirationIdV1>("/v1/muse/inspiration/{inspiration_id}", localVarRequestOptions, this.Configuration);

            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ResponsePutMuseInspirationUsingInspirationIdV1</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1>> PutMuseInspirationUsingInspirationIdV1Async(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = PutMuseInspirationUsingInspirationIdV1WithHttpInfoAsync(inspirationId, updateInspirationRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> localVarResponse = await task.ConfigureAwait(false);
#else
            Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> localVarResponse = await task;
#endif
            return localVarResponse;
        }

        /// <summary>
        /// Update Inspiration Update an existing inspiration in the database.  Args:     request: FastAPI request object.     inspiration_id: ID of the inspiration to update.     body: Updated inspiration object.  Returns: Updated inspiration or error response.
        /// </summary>
        /// <exception cref="Unity.Muse.Chat.BackendApi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inspirationId"></param>
        /// <param name="updateInspirationRequest"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ResponsePutMuseInspirationUsingInspirationIdV1)</returns>
        public async System.Threading.Tasks.Task<Unity.Muse.Chat.BackendApi.Client.ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1>> PutMuseInspirationUsingInspirationIdV1WithHttpInfoAsync(string inspirationId, UpdateInspirationRequest updateInspirationRequest, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'inspirationId' is set
            if (inspirationId == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'inspirationId' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationIdV1");

            // verify the required parameter 'updateInspirationRequest' is set
            if (updateInspirationRequest == null)
                throw new Unity.Muse.Chat.BackendApi.Client.ApiException(400, "Missing required parameter 'updateInspirationRequest' when calling MuseChatBackendApi->PutMuseInspirationUsingInspirationIdV1");


            Unity.Muse.Chat.BackendApi.Client.RequestOptions localVarRequestOptions = new Unity.Muse.Chat.BackendApi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Unity.Muse.Chat.BackendApi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("inspiration_id", Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToString(inspirationId)); // path parameter
            localVarRequestOptions.Data = updateInspirationRequest;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.HeaderParameters.Add("access_token", this.Configuration.GetApiKeyWithPrefix("access_token"));
            }
            // authentication (APIKeyQuery) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("access_token")))
            {
                localVarRequestOptions.QueryParameters.Add(Unity.Muse.Chat.BackendApi.Client.ClientUtils.ParameterToMultiMap("", "access_token", this.Configuration.GetApiKeyWithPrefix("access_token")));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PutAsync<ResponsePutMuseInspirationUsingInspirationIdV1>("/v1/muse/inspiration/{inspiration_id}", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            return localVarResponse;
        }

    }
}
