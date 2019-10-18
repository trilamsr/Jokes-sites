using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt_Exam.Models;
using Microsoft.AspNetCore.Identity;
using Belt_Exam.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Belt_Exam.Utilities
{
    static public class TimeUtilities
    {
        static public DateTime CurrentTime()
        {
            return DateTime.Now;
        }

        static public string Greeting()
        {
            int hours = CurrentTime().Hour;
            if (hours >= 0 && hours <= 12) {
                return "Good Morning";
            } else if (hours >= 12 && hours <= 16) {
                return "Good Afternoon";
            } else if (hours >= 16 && hours <= 21) {
                return "Good Evening";
            } else {
                return "Good Night";
            }
        }
    }
}
