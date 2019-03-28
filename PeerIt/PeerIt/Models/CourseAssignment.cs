using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class CourseAssignment
    {
        #region Constructors

        public CourseAssignment(string Name, int CourseID) 
        {
           
        }
        #endregion Constructors
        #region Variables and Properties
        private int CourseAssignmentID { get; set; }
        private string CouseAssignmentName { get; set; }
        private string InstructionText { get; set; }
        private string InstructionsUrl { get; set; }
        private string RubricText { get; set; }
        private string RubricUrl { get; set; }
        #endregion Variables and Properties
           #region Methods
        // return types are currently returning a placeholder - future code will be more dynamic.
        public bool SetAssignmentName(string name) 
        {
            return false;
        }
        public bool SetInstructionText(string text) 
        {
            return false;
        }
        public bool SetInstructionUrl(string urlText) 
        {
            return false;
        }
        public bool SetRubricText(string text) 
        {
            return false;
        }
        public bool SetRubricUrl(string urlText) 
        {
            return false;
        }
        public string GetName() 
        {
            return "name";
        }
        public string GetInstructiontext() 
        {
            return "instructions";
        }
        public string GetInstructionUrl() 
        {
            return "instructionUrl";
        }
        public string GetRubricText() 
        {
            return "Rubric";
        }
        public string GetRubricUrl() 
        {
            return "rubricUrl";
        }
        public int GetCourseID() 
        {
            return -1;
        }
        #endregion Methods
    }
}
