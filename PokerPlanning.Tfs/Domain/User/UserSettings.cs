namespace PokerPlanning.Tfs.Domain.User
{
    using System;

    public class UserSettings
    {
        public Guid UserId { get; protected set; }

        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string IterationName { get; set; }
        public string IterationPath { get; set; }
        public string AreaName { get; set; }
        public string AreaPath { get; set; }
        
        public UserSettings(Guid id)
        {
            UserId = id;
        }        
    }
}
