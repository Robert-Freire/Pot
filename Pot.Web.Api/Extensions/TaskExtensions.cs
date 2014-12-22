// Type: Microsoft.AspNet.Identity.TaskExtensions
// Assembly: Microsoft.AspNet.Identity.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C3D1FD63-164F-4540-A6FA-39DF57036A66
// Assembly location: C:\Users\robert.freire\AppData\Roaming\lib\Microsoft.AspNet.Identity.Core.2.1.0\lib\net45\Microsoft.AspNet.Identity.Core.dll

namespace Pot.Web.Api
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The task extensions.
    /// Copied from Microsoft.AspNet.Identity.TaskExtensions. Microsoft.AspNet.Identity.Core.dll
    /// Take a look also for  http://blogs.msdn.com/b/pfxteam/archive/2012/02/29/10274035.aspx (Await pattern)
    /// </summary>
    public static class TaskExtensions
    {
        public static TaskExtensions.CultureAwaiter<T> WithCurrentCulture<T>(this Task<T> task)
        {
            return new TaskExtensions.CultureAwaiter<T>(task);
        }

        public static TaskExtensions.CultureAwaiter WithCurrentCulture(this Task task)
        {
            return new TaskExtensions.CultureAwaiter(task);
        }

        public struct CultureAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
        {
            private readonly Task<T> task;



            public CultureAwaiter(Task<T> task)
            {
                this.task = task;
            }

            public bool IsCompleted
            {
                get
                {
                    return this.task.IsCompleted;
                }
            }
            public TaskExtensions.CultureAwaiter<T> GetAwaiter()
            {
                return this;
            }

            public T GetResult()
            {
                return this.task.GetAwaiter().GetResult();
            }

            public void OnCompleted(Action continuation)
            {
                throw new NotImplementedException();
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                this.task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted((Action)(() =>
                {
                    CultureInfo local_0 = Thread.CurrentThread.CurrentCulture;
                    CultureInfo local_1 = Thread.CurrentThread.CurrentUICulture;
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    try
                    {
                        continuation();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentCulture = local_0;
                        Thread.CurrentThread.CurrentUICulture = local_1;
                    }
                }));
            }
        }

        public struct CultureAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            private readonly Task task;

            public CultureAwaiter(Task task)
            {
                this.task = task;
            }
            public bool IsCompleted
            {
                get
                {
                    return this.task.IsCompleted;
                }
            }


            public TaskExtensions.CultureAwaiter GetAwaiter()
            {
                return this;
            }

            public void GetResult()
            {
                this.task.GetAwaiter().GetResult();
            }

            public void OnCompleted(Action continuation)
            {
                throw new NotImplementedException();
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                this.task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(
                    (Action)(() =>
                    {
                        CultureInfo local_0 = Thread.CurrentThread.CurrentCulture;
                        CultureInfo local_1 = Thread.CurrentThread.CurrentUICulture;
                        Thread.CurrentThread.CurrentCulture = currentCulture;
                        Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                        try
                        {
                            continuation();
                        }
                        finally
                        {
                            Thread.CurrentThread.CurrentCulture = local_0;
                            Thread.CurrentThread.CurrentUICulture = local_1;
                        }
                    }));
            }
        }
    }
}
