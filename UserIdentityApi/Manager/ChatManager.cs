using ChatLibrary.Entities;
using ChatLibrary.Models;
using IdentityApi.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace IdentityApi.Manager
{
    public class ChatManager //IHubContext<ConversationHub> conversationHubContext
    {
        private readonly AppDbContext _dbContext;
        //private readonly IHubContext<ConversationHub> _conversationHubContext;

        public ChatManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ChatModel>> GetConversations(Guid userId)
        {
            var conversations = await _dbContext.ChatGroups
                .Where(conversation => conversation.UserIds.Contains(userId))
                .ToListAsync();

            return conversations.Select(conversation => new ChatModel()
            {
                FromUserId = conversation.UserIds.First(u => u != userId),
                Id = conversation.Id
            }).ToList();
        }

        public async Task<List<MessageModel>> GetConversationMessages(Guid conversationId)
        {
            var messages = await _dbContext.Messages
                .Where(m => m.ChatGroupId == conversationId)
                .ToListAsync();

            return messages.Select(message => new MessageModel()
            {
                Date = DateTime.UtcNow,
                FromUserId = message.FromId,
                Text = message.Text,
                Id = message.Id
            }).ToList();
        }

        public async Task SaveMessage(Guid userId, NewMessageModel messageModel)
        {
            var conversation = await _dbContext.ChatGroups
                .Where(c =>
                    c.UserIds.Contains(userId)
                    && c.UserIds.Contains(messageModel.ToUserId))
                .FirstOrDefaultAsync();

            if (conversation == null)
            {
                conversation = new ChatGroup()
                {
                    UserIds = new List<Guid>() { userId, messageModel.ToUserId }
                };

                _dbContext.ChatGroups.Add(conversation);
                await _dbContext.SaveChangesAsync();
            }

            var message = new Message()
            {
                ChatGroupId = conversation.Id,
                CreatedTime = DateTime.UtcNow,
                FromId = userId,
                Text = messageModel.Text
            };

            //send to hub

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            //await SendMessagesToHub(conversation.UserIds, new MessageModel(message));
        }

        //private async Task SendMessagesToHub(
        //    List<Guid> userIds,
        //    MessageModel messageModel)
        //{
        //    await _conversationHubContext.Clients
        //        .Users(userIds.Select(u => u.ToString()).ToList())
        //        .SendAsync("NewMessage", messageModel);
        //}
    }
}
