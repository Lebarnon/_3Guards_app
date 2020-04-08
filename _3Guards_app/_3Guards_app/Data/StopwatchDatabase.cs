using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using _3Guards_app.Models;
using System.Threading.Tasks;

namespace _3Guards_app.Data
{
    public class StopwatchDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public StopwatchDatabase(string dppath)
        {
            _database = new SQLiteAsyncConnection(dppath);

            //Create tables here 

            _database.CreateTableAsync<Result>().Wait();
            _database.CreateTableAsync<Timing>().Wait();
        }

        //Get the WHOLE result table as a list
        public Task<List<Result>> GetResultsAsync()
        {
            return _database.Table<Result>().ToListAsync();
        }


        //Get the INDIVIDUAL results from result table
        public Task<Result> GetResultAsync(int id)
        {
            return _database.Table<Result>().Where(i => i.ID == id).FirstOrDefaultAsync(); 
        }


        //Creates a new result in result table or update 
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



        //FOR TIMING//
        //Get the WHOLE timing table as a list
        public Task<List<Timing>> GetTimingsAsync()
        {
            return _database.Table<Timing>().ToListAsync();
        }


        //Get the INDIVIDUAL timing from result table
        public Task<Timing> GetTimingAsync(int id)
        {
            return _database.Table<Timing>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }


        //Creates a new timing in timing table 
        public Task<int> SaveResultAsync(Timing timing)
        {
            if (timing.ID != 0)
            {
                return _database.UpdateAsync(timing);
            }
            else
            {
                return _database.InsertAsync(timing);
            }
        }
        public Task<int> DeleteTimingAsync(Timing timing)
        {
            return _database.DeleteAsync(timing);
        }
       
    }
}