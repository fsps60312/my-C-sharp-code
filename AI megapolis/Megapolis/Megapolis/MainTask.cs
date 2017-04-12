using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Windows.Forms;
using Motivation;
using System.Drawing;

namespace Megapolis
{
    class MainTask:MyTask
    {
        bool pauseRegularTasks = false;
        public void RunManualSelectedTasks()
        {
            TaskCheckedListForm f = new TaskCheckedListForm(tasks);
            f.Show();
            f.OKButtonClicked += new TaskCheckedListForm.OKButtonClickedEventHandler((List<MyTask> subt) =>
            {
                var result = MessageBox.Show("Manual selected tasks can't be stopped in the mean time, please do not do anything before finishing\r\nPlease make sure regular tasks are running\r\nContinue?", "Warning", MessageBoxButtons.OKCancel);
                if (result != DialogResult.OK)
                {
                    log = "Manual selected tasks canceled";
                    return;
                }
                Thread thread = new Thread(() =>
                {
                    log = "Running manual selected tasks, please do not do anything before finishing...";
                    //foreach(var t in subt)
                    //{
                    //    status = t.ToString();
                    //}
                    //status = "That's all";
                    pauseRegularTasks = true;
                    int timeOut = 60000;
                    log = "Waiting for regular tasks to pause...";
                    log = $"If regular tasks isn't running, the process would time out in {timeOut} ms...";
                    for (DateTime startTime = DateTime.Now; pauseRegularTasks == true && (DateTime.Now - startTime).TotalMilliseconds < timeOut;) Thread.Sleep(500);
                    if (pauseRegularTasks)
                    {
                        log = $"Time out. This might be because regular tasks isn't running";
                        log = "Task terminated";
                        pauseRegularTasks = false;
                    }
                    else
                    {
                        var t = new BatchRunOnceTask(subt);
                        Add(t);
                        try
                        {
                            t.RunScript();
                        }
                        catch (Exception error)
                        {
                            log = $"Catch Error:\r\n{error}";
                        }
                        System.Diagnostics.Trace.Assert(tasks.Remove(t));
                        pauseRegularTasks = true;
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            });
        }
        public KeyValuePair<DateTime,string> ShowNextRunTime()
        {
            var nextTaskToRun = NextTaskToRun();
            log = $"Next Task to Run: \"{nextTaskToRun.Value}\" in ({nextTaskToRun.Key - DateTime.Now})";
            return nextTaskToRun;
        }
        public List<KeyValuePair<DateTime,string>>ShowTaskInfo()
        {
            List<KeyValuePair<DateTime, string>> answer = new List<KeyValuePair<DateTime, string>>();
            foreach (MyTask t in tasks) answer.Add(new KeyValuePair<DateTime, string>(t.nextRunTime, t.ToString()));
            answer.Sort(new Comparison<KeyValuePair<DateTime, string>>((a, b) => { return a.Key == b.Key ? 0 : (a.Key < b.Key ? -1 : 1); }));
            foreach(var p in answer)
            {
                log = $"\"{p.Value}\": in ({p.Key - DateTime.Now})";
            }
            return answer;
        }
        public override void RunScript()
        {
            HashSet<string> errorScripts = new HashSet<string>();
            log = $"Started";
            ShowNextRunTime();
            stopping = false;
            while (!stopping)
            {
                if(pauseRegularTasks)
                {
                    log = "Regular tasks paused";
                    pauseRegularTasks = false;
                    while (!pauseRegularTasks) Thread.Sleep(1000);
                    log = "Regular tasks resumed";
                    pauseRegularTasks = false;
                }
                if (NextTaskToRun().Key <= DateTime.Now)
                {
                    if (StartBlueStacksIfDidnt() && StartMegapolisIfDidnt())
                    {
                        RunExpiredScripts(ref errorScripts);
                        var nextTaskToRun = ShowNextRunTime();
                        if ((nextTaskToRun.Key - DateTime.Now).TotalMilliseconds > Constant.MaxTimeIntervalBetweenTasksToNotCloseBlueStacks)
                        {
                            log = "Still long to the next task, closing BlueStacks...";
                            CloseBlueStacks();
                        }
                    }
                    else
                    {
                        CloseBlueStacks();
                        log = $"Waiting for {Constant.TimeToSleepIfBlueStacksOrMegapolisDidntStartSuccessfully} ms and try again...";
                        Thread.Sleep(Constant.TimeToSleepIfBlueStacksOrMegapolisDidntStartSuccessfully);
                    }
                }
                Thread.Sleep((int)timeSpan.TotalMilliseconds);
            }
            stopping = false;
            //log = "Closing BlueStacks if it is still running...";
            //CloseBlueStacks();
            log = $"Stopped";
        }
        public MainTask(TimeSpan checkInterval):base("Main", checkInterval)
        {
            Add(new ArchaeologicalResearchLabTask());
            Add(new ArmoredVehicleFactoryTask());
            Add(new ArmsRaceTask());
            Add(new AtmosphericProbeTask());
            Add(new AviationDesignCompanyTask());
            Add(new CargoRocketTask());
            Add(new CenterOfGravitationalResearchTask());
            Add(new CreativeProjectCenterTask());
            Add(new DeepSubmergenceVehicleTask());
            Add(new DeleteInactiveFriendsTask());
            Add(new DrillingMachineTask());
            Add(new GeocryologyInstituteTask());
            Add(new GoldDragonCasinoTask());
            Add(new HotelBelizeTask());
            Add(new HoustonTask());
            Add(new IdahoBusinessCenterIowaBusinessCenterTask());
            Add(new InstituteOfFutureDevelopmentTask());
            Add(new InstituteOfNaturalPhenomenaTask());
            Add(new InterstellarLinerTask());
            Add(new JapaneseCulturalInstituteTask());
            Add(new Level6BusinessJetVIPJetTask());
            Add(new Level6CargoPlaneTask());
            Add(new Level6LongRangeAirlinerTask());
            Add(new Level6PassengerAirlinerTask());
            Add(new LongRangeAirlinerTask());
            Add(new MannedRocketTask());
            Add(new MilitaryAircraftFactoryTask());
            Add(new MilitaryShipyardTask());
            Add(new MissileConstructionFactoryTask());
            Add(new MontrealBusinessCenterVancouverOfficeCenterTask());
            Add(new NorthStationTask());
            Add(new OceanicStationTask());
            Add(new OrbitalShuttleTask());
            Add(new PrimeFinanceCenterTask());
            Add(new RailroadTask());
            Add(new ResearchCenterInMegapolisTask());
            Add(new ResearchCenterInRockyMountainsTask());
            Add(new ReserveAdministrationTask());
            Add(new ScientificResourceCenterTask());
            Add(new SeismicPredictionLaboratoryTask());
            Add(new SendGiftsTask());
            Add(new SpaceShipTask());
            Add(new SubmarineFactoryTask());
            Add(new TianningResidenceTask());
            Add(new TourAdministrationTask());
            Add(new TreasureChestTask());
            Add(new TreasuryInMegapolisTask());
        }
    }
}
