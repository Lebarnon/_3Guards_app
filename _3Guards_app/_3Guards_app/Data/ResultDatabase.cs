using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using _3Guards_app.Models;
using System.Threading.Tasks;

namespace _3Guards_app.Data
{
    public class ResultDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ResultDatabase(string dppath)
        {
            _database = new SQLiteAsyncConnection(dppath);
            _database.CreateTableAsync<Result>().Wait();
        }

        //Get the WHOLE results database
        public Task<List<Result>> GetResultsAsync()
        {
            return _database.Table<Result>().ToListAsync();
        }


        //Get the INDIVIDUAL results
        public Task<Result> GetResultAsync(int id)
        {
            return _database.Table<Result>().Where(i => i.ID == id).FirstOrDefaultAsync(); //how they com up with this? i.ID?
        }


        //Creates a new result in database or update 
        public Task<int> SaveResultAsync(Result result)
        {
            if (result.ID != 0)
            {
                return _database.UpdateAsync(result);
            }
            else
            {
                return _database.InsertAsync(result);
            }
        }

        public Task<int> DeleteResultAsync(Result result)
        {
            return _database.DeleteAsync(result);
        }
    }
}