namespace PokerPlanning.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Server;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using PokerPlanning.Tfs.Domain.User;

    public class UserContext
    {
        private static readonly string CONTEXT_KEY = "USER_CONTEXT";
        private string _projectName;
        private List<string> _users;

        public Guid Id { get; set; }
        public TfsTeamProjectCollection TfsCollection { get; set; }
        public WorkItemStore WorkItemStore { get; set; }
        public VersionControlServer VersionControlServer { get; private set; }
        public ProjectDetails CurrentProject { get; private set; }
        public List<string> ProjectNames { get; private set; }
        public UserSettings Settings { get; private set; }
        public string Name { get; private set; }

        public IEnumerable<string> Users
        {
            get
            {
                return _users;
            }
            private set
            {
                _users = new List<string>(value);
            }
        }

        public static UserContext Current
        {
            get
            {                
                
                UserContext context = Cache.Get(CONTEXT_KEY) as UserContext;
                if (context == null)
                {
                    context = new UserContext();
                    Cache.Insert(CONTEXT_KEY, context);

                    return context;
                }

                return context;
            }
        }

        internal UserContext()
        {
            if (String.IsNullOrEmpty(AppSettings.TfsServer))
                throw new ArgumentNullException("The TfsServer settings are not specified, please set it in config file");

            if (String.IsNullOrEmpty(AppSettings.DefaultProjectName))
                throw new ArgumentNullException("The DefaultProjectName settings is empty, please set it in the config file");

            // Connect to TFS
            _users = new List<string>();

            NetworkCredential credentials = new NetworkCredential("login", "password", "domain");
            TfsCollection = new TfsTeamProjectCollection(new Uri(AppSettings.TfsServer), credentials);

            ICredentials creadentials = TfsCollection.Credentials;

            //TfsCollection.Authenticate();
            WorkItemStore = new WorkItemStore(TfsCollection);
            VersionControlServer = TfsCollection.GetService<VersionControlServer>();
            
            // Get the current username, and load their settings
            Name = TfsCollection.AuthorizedIdentity.DisplayName;
            Id = TfsCollection.AuthorizedIdentity.TeamFoundationId;

            Settings = new UserSettings(Id);
            Settings.Name = Name;

            // Set the current project and the view settings
            if (String.IsNullOrEmpty(Settings.ProjectName) || !WorkItemStore.Projects.Contains(Settings.ProjectName))
            {
                ChangeCurrentProject(AppSettings.DefaultProjectName);
            }
            else
            {
                _projectName = Settings.ProjectName;
                CurrentProject = new ProjectDetails(WorkItemStore.Projects[_projectName]);
            }

            PopulateProjectNames();
            PopulateUsers();
        }

        private void PopulateUsers()
        {
            Users = new List<string>();

            IGroupSecurityService service = TfsCollection.GetService<IGroupSecurityService>();

            Identity usersInCollection = service.ReadIdentity(SearchFactor.AccountName, "Project Collection Valid Users", QueryMembership.Expanded);
            Identity[] members = service.ReadIdentities(SearchFactor.Sid, usersInCollection.Members, QueryMembership.Expanded);

            for (int i = 0; i < members.Length; i++)
            {
                if (!members[i].SecurityGroup)
                {
                    string name = members[i].DisplayName;
                    if (string.IsNullOrEmpty(name))
                        name = members[i].AccountName;

                    _users.Add(name);
                }
            }

            _users.Sort();
        }

        private void PopulateProjectNames()
        {
            ProjectNames = new List<string>();
            foreach (Project project in WorkItemStore.Projects)
            {
                ProjectNames.Add(project.Name);
            }
        }

        private void ChangeCurrentProject(string projectName)
        {
            if (String.IsNullOrWhiteSpace(projectName))
                throw new ArgumentNullException("The project name was null or empty");

            if (!WorkItemStore.Projects.Contains(projectName))
                throw new InvalidOperationException(string.Format("The project {0} doesn't exist", projectName));

            _projectName = projectName;
            CurrentProject = new ProjectDetails(WorkItemStore.Projects[projectName]);

            Settings.ProjectName = projectName;
            Settings.IterationName = CurrentProject.Iterations[0].Name;
            Settings.IterationPath = CurrentProject.Iterations[0].Path;
        }
    }
}
