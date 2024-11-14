namespace Dating.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
        ILikesRepository LikesRepository { get; }
        IMessagesRepository MessagesRepository { get; }
        IGroupsRepository GroupsRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
