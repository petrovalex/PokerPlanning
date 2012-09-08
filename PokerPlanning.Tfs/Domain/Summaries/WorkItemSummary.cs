namespace PokerPlanning.Tfs.Domain
{
    using System;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System.Runtime.Serialization;

    public class WorkItemSummary
    {
        private WorkItemType _workItemType { get; set; }

        public int Id { get; set; }
        
        public string ProjectName { get; set; }

        public int AreaId { get; set; }
        
        public string AreaPath { get; set; }
        
        public int IterationId { get; set; }
        
        public string IterationPath { get; set; }

        public int OriginalEstimate { get; set; }

        public string Title { get; set; }
       
        public string WorkItemType
        {
            get { return _workItemType.Name; }            
        }

        private WorkItemSummary()
        {

        }

        public static WorkItemSummary FromWorkItem(WorkItem item)
        {
            var summary = new WorkItemSummary
            {
                Id = item.Id,
                ProjectName = item.Project.Name,
                AreaId = item.AreaId,
                AreaPath = item.AreaPath,
                IterationId = item.IterationId,
                IterationPath = item.IterationPath,
                Title = item.Title,
                _workItemType = item.Type
            };   
         
            return summary;
        }
    }
}
