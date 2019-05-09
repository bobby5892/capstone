using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    /*
    The following classes are used for modelling return data to students.
    Currently, the data being used by the controllers contains more info
    than what a student should be allowed to have. Therefore, these models
    take the data relevant to the student, leaving out the data that is
    potentially a security risk.
    */
    public class CourseDataOut
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        
        public string FK_INSTRUCTOR_NAME { get; set; }
    }

    public class CourseAssignmentDataOut
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public int FK_COURSE_ID { get; set; }
        
        public string InstructionText { get; set; }
        
        public string InstructionsUrl { get; set; }
        
        public string RubricText { get; set; }
        
        public string RubricUrl { get; set; }
    }

    public class StudentAssignmentDataOut
    {
        public int ID { get; set; }
        
        public int CourseAssignment_ID { get; set; }
        
        public string Student_Name { get; set; }

        public DateTime TimestampCreated { get; set; }
        
        public string Content { get; set; }
        
        public string Status { get; set; }
        
        public int Score { get; set; }
    }

    public class ReviewDataOut
    {
        public int ID { get; set; }
        
        public AppUser FK_APP_USER { get; set; }
        
        public StudentAssignment FK_STUDENT_ASSIGNMENT { get; set; }
        
        public string Content { get; set; }

        public DateTime TimestampCreated { get; set; }
    }

    public class CommentDataOut
    {
        public int ID { get; set; }
        
        public int FK_STUDENT_ASSIGNMENT_ID { get; set; }
        
        public DateTime TimestampCreated { get; set; }
        
        public string Content { get; set; }
    }

    public class ActiveReviewerDataOut
    {
        public int ID { get; set; }

        public string FK_APP_USER_VIEWER { get; set; }

        public int FK_STUDENT_ASSIGNMENT_ID { get; set; }
    }
}
