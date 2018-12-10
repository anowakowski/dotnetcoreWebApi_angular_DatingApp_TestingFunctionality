using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepostiory : IDatingRepository
    {
        private readonly DataContext dataContext;
        public DatingRepostiory(DataContext dataContext)
        {
            this.dataContext = dataContext;

        }
        public void Add<T>(T entity) where T : class
        {
            dataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            dataContext.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            var mainPhoto = await dataContext.Photos.Where(u => u.Id == userId).ToListAsync();;
            
            //.FirstOrDefaultAsync(p => p.IsMain);
            
            return mainPhoto.FirstOrDefault(x => x.IsMain == true);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await dataContext.Photos.FirstOrDefaultAsync(x => x.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = dataContext.Users.Include(p => p.Photos).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }
 
        public async Task<Photo> GetMainPhoto(int id)
        {
            return await dataContext.Photos.Where(x => x.UserId == id).FirstOrDefaultAsync(x => x.IsMain);
        }
    }
}