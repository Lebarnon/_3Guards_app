using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLitePCL;
using _3Guards_app.Models;
using System.Threading.Tasks;
using SQLiteNetExtensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace _3Guards_app.Data
{
    public class GuardsDatabase 
    {
        readonly SQLiteAsyncConnection _database = null;
        
        public GuardsDatabase(string dppath)
        {
            _database = new SQLiteAsyncConnection(dppath);

            //Create tables here 

            _database.CreateTableAsync<Result>().Wait();
            _database.CreateTableAsync<Timing>().Wait();

            _database.CreateTableAsync<Erac>().Wait();
            _database.CreateTableAsync<EracUser>().Wait();
        }

        //for testing purpose only
        public void Reset() 
        {
            _database.DropTableAsync<Result>().Wait();
            _database.DropTableAsync<Timing>().Wait();

            _database.DropTableAsync<Erac>().Wait();
            _database.DropTableAsync<EracUser>().Wait();


            _database.CreateTableAsync<Result>().Wait();
            _database.CreateTableAsync<Timing>().Wait();

            _database.CreateTableAsync<Erac>().Wait();
            _database.CreateTableAsync<EracUser>().Wait();
        }

        public void CheckTables()
        {
            var isResultTable = GetResultsAsync();
            var isTimingTable = GetAllTimingsAsync();
            if (isResultTable == null)
            {
                _database.CreateTableAsync<Result>().Wait();
            }
            else if (isTimingTable == null)
            {
                _database.CreateTableAsync<Timing>().Wait();
            }
            return;
          
            //bool ResultTableExist = _database.GetTableInfoAsync(App.Database.Table<Result>.ToString) ;
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

        public async Task<int> DeleteResultAsync(Result result)
        {
            var timings = await App.Database.GetTimingsAsync(result.ID);
            foreach (var Timing in timings)
            {
                await _database.DeleteAsync(Timing);
            }
            return await _database.DeleteAsync(result);
        }

        public Task PopulateResultTimingList(Result result)
        {
            return _database.UpdateWithChildrenAsync(result);
        }



        //FOR TIMING//
        //Get the Whole timing table for ANY result
        public Task<List<Timing>> GetAllTimingsAsync()
        {
            return _database.Table<Timing>().ToListAsync();
        }
        //Get the WHOLE timing table for a specific result as a list
        public Task<List<Timing>> GetTimingsAsync(int id)
        {
            return _database.Table<Timing>().Where(i => i.ResultID == id).ToListAsync();
        }

            
        //Get the INDIVIDUAL timing from result table
        public Task<Timing> GetTimingAsync(int id)
        {
            return _database.Table<Timing>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }


        //Creates a new timing in timing table 
        public Task<int> SaveTimingAsync(Timing timing)
        {
            return _database.InsertAsync(timing);
        }
        public Task<int> DeleteTimingAsync(Timing timing)
        {
            return _database.DeleteAsync(timing);
        }
       
    }
}