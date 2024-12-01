using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;

namespace Dating.DAL.Repositories
{
    public class UnitOfWork(DataContext dataContext,
                            IUsersRepository usersRepository,
                            ILikesRepository likesRepository,
                            IMessagesRepository messagesRepository,
                            IGroupsRepository groupsRepository,
                            IPhotosRepository photoRepository
                            ) : IUnitOfWork
    {
        public IUsersRepository UsersRepository => usersRepository;
        public ILikesRepository LikesRepository => likesRepository;
        public IMessagesRepository MessagesRepository => messagesRepository;
        public IGroupsRepository GroupsRepository => groupsRepository;
        public IPhotosRepository PhotoRepository => photoRepository;

        public async Task<bool> Complete()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return dataContext.ChangeTracker.HasChanges();
        }
    }
}
