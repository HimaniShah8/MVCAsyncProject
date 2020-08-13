using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sample_Async_Proj.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// This function has hiearchery level 
        /// </summary>
        /// <returns></returns>
        public async  Task<ActionResult> AsyncCallStart()
        {
            //var res=await AsyncCall().ConfigureAwait(false);
            var res =AsyncCall();
            Debug.WriteLine(res);
            Debug.WriteLine(DateTime.Now);
            return View();
        }

        public async Task<string> AsyncCall()
        {
            var time = DateTime.Now;
            Debug.WriteLine(time);
            //Calling async function 1 
          
            var asynccall1 =await AsyncFunction1("Async1 starts").ConfigureAwait(false);
            Debug.WriteLine(asynccall1);
            ViewBag.Time = DateTime.Now;
            Debug.WriteLine(DateTime.Now);
           
            var asynccall2 = AsyncFunction2("Async2 starts");
            Debug.WriteLine(DateTime.Now);
            //when we start new thread if we want to get result from function1 there are 2 ways either function1 variable.result or await  functoin1
            //If we want do something with result in this function then we can call it like await function1(param)
            var res=asynccall1;
            Debug.WriteLine(res);

            //Await for result.
            var res2=await asynccall2;
           
            Debug.WriteLine(res2);
            return  "asynccall done";
        }

       
        private async Task<string> AsyncFunction1(string str1)
        {
            Debug.WriteLine(str1);
            Debug.WriteLine("thread1 will sleep for 10 second.");
            Debug.WriteLine(DateTime.Now);
            
            //with configure await false it will delay this thread but main thread i.e. asyncCall will continue its execution when this thread is asleep           
            await Task.Delay(10000).ConfigureAwait(false);
            //Thread.Sleep(10000);
            Debug.WriteLine(DateTime.Now);
            Debug.WriteLine("Thread1 Done sleep");
            return "Async 1 done";
        }
        private async Task<string> AsyncFunction2(string str2)
            {
            Debug.WriteLine("thread2 will sleep for 10 second.");
            Debug.WriteLine(str2);
            //Here since we are awaiting for delay to comeplte next function will be called after delay
             await Task.Delay(10000);

            var asyncfunc3 =  AsyncFunction3("Async func 3 call");
            
            for (var j = 0; j < 20; j++)
            {
                Debug.WriteLine(j + "j");
            }          
            Debug.WriteLine(DateTime.Now);
           
            //Wait for result to write
            var res3=asyncfunc3.Result;
            Debug.WriteLine(res3);
            string resl = "Async 2 done";
            return  resl;

        }

        private async Task<string> AsyncFunction3(string str3)
        {
        Debug.WriteLine(str3);

            await Task.Delay(10000).ConfigureAwait(false);

            for(var i=0;i<20;i++)
            {
             Debug.WriteLine(i + "i");
            }
            Debug.WriteLine("Thread3 done executing");
            return "Thread3 done executing ";

        }

        //Calling all async function in different way from one function only to check await operation
        public async Task<ActionResult> GetListAsync()
        {
            //Create a stopwatch for getting excution time  
            var watch = new Stopwatch();
            watch.Start();
            //When await is used it will wait for execution to complete.
            var country =await GetCountryAsync();
            //This will start process on new threas as we are not waiting for the result
            var state = GetStateAsync();
            //This will start process on new threas as we are not waiting for the result
            var city = GetCityAsync();
            var aa =await GetAAAsync().ConfigureAwait(false);
            var content =  country;
            var count = await state;

            var name =  city.Result;
            watch.Stop();
            ViewBag.WatchMilliseconds = watch.ElapsedMilliseconds;
            ViewBag.country = content;
            ViewBag.prov = count;
            ViewBag.city = name;

            return View();
        }

        public async Task<string> GetAAAsync()
        {
            await Task.Delay(2000).ConfigureAwait(false);
            return "aa";
        }

        #region-- > GetCountry Method
        public async Task<string> GetCountryAsync()
        {
            await Task.Delay(3000); //Use - when you want a logical delay without blocking the current thread.  
            return "Canada";
        }
        #endregion
        #region-- > GetState Method

        public async Task<string> GetStateAsync()
        {
            await Task.Delay(5000); //Use - when you want a logical delay without blocking the current thread.  
            return "Ontario";
        }
        #endregion
        #region-- > GetCity Method

        public async Task<string> GetCityAsync()
        {
            await Task.Delay(3000).ConfigureAwait(false); //Use - when you want a logical delay without blocking the current thread.  
            return "Mississauga";
        }
        #endregion
    }
}