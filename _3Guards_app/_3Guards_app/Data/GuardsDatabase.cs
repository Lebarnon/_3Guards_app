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
            _database.CreateTableAsync<EracQues>().Wait();
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
           
            if (isResultTable == null)
            {
                _database.CreateTableAsync<Result>().Wait();
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
            var timings = GetResultTimingList(result.ID).Result;
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
        
        public Task<List<Timing>> GetResultTimingList(int id)
        {
            return _database.Table<Timing>().Where(i => i.ResultID == id).ToListAsync();
        }


        public Task<int> SaveTimingAsync(Timing timing)
        {
            return _database.InsertAsync(timing);
        }
        public Task<int> DeleteTimingAsync(Timing timing)
        {
            return _database.DeleteAsync(timing);
        }



        // FOR ERAC
        //
        //

        //Get the WHOLE Eracs table as a list
        public Task<List<Erac>> GetEracsAsync()
        {
            return _database.Table<Erac>().ToListAsync();
        }


        //Get the INDIVIDUAL results from result table
        public Task<Erac> GetEracAsync(int id)
        {
            return _database.Table<Erac>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }


        //Creates a new result in result table or update 
        public Task<int> SaveEracAsync(Erac erac)
        {
            if (erac.ID != 0)
            {
                return _database.UpdateAsync(erac);
            }
            else
            {
                return _database.InsertAsync(erac);
            }
        }

        public async Task<int> DeleteResultAsync(Erac erac)
        {
            var eracUsers = GetEracEracUserList(erac.ID).Result;
            foreach (var eracUser in eracUsers)
            {
                await _database.DeleteAsync(eracUser);
            }
            return await _database.DeleteAsync(erac);
        }

        public Task PopulateEracEracUserList(Erac erac)
        {
            return _database.UpdateWithChildrenAsync(erac);
        }

        public Task PopulateEracEracQues(Erac erac)
        {
            return _database.UpdateWithChildrenAsync(erac);
        }

        public Task<List<EracUser>> GetEracEracUserList(int id)
        {
            return _database.Table<EracUser>().Where(i => i.EracID == id).ToListAsync();
        }


        public Task<int> SaveEracUserAsync(EracUser eracUser)
        {
            return _database.InsertAsync(eracUser);
        }
        public Task<int> DeleteEracUserAsync(EracUser eracUser)
        {
            return _database.DeleteAsync(eracUser);
        }

    }
}