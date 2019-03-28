using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class Course
    {
        #region Constructors
        public Course(string name, bool isActive, int InstructorID) 
        {

        }
        #endregion Constructors

        #region Variables and Properties
        public int CourseID { get; set; }
        private string CourseName { get; set; }
        private bool IsActive { get; set; }

        #endregion Variables and Properties

        #region Methods

        // All methods are currently returning placeholders

        public bool SetCourseName(string name) 
        {
            return false;
        }
        public bool SetActive(bool status) 
        {
            return false;
        }
        public bool SetInstructor(int instructorID) 
        {
            return false;
        }
        public string GetCourseName() 
        {
            return "name";
        }
        public bool GetActive() 
        {
            return false;
        }
        public int GetInstructorID() 
        {
            return -1;
        }
        #endregion Methods
    }
}
