using Hangfire;
using IdentityMicroservice.Infrastructure.Services.Cronjobs.ResumeTrainingOnModelCronJob;

namespace IdentityMicroservice.Infrastructure.Services.Cronjobs
{
    public static  class JobSchedulers
    {
        public static void ScheduleJobs()
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate<ResumeTrainingOnModel>(ResumeTrainingOnModel.GetJobName(), job => job.Execute(), Cron.Daily(10));
        }
    }
}
