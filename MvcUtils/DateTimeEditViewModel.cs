
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MvcUtils
{
  public class DateTimeEditViewModel
  {
    public static readonly int MaxYear = DateTime.Now.Year;
    public const int MinYear = 1950;


    [Required]
    [Range(1, 12)]
    public int Month { get; set; }

    [Required]
    [Range(1, 31)]
    public int Day { get; set; }

    [Required]
    [Range(MinYear, 2100)]
    public int Year { get; set; }

    public DateTimeEditViewModel() : this(DateTime.Now) { }

    public DateTimeEditViewModel(DateTime value)
    {
      Month = value.Month;
      Day = value.Day;
      Year = value.Year;
    }

    public DateTime ToDateTime() { return new DateTime(Year, Month, Day); }

    public static implicit operator DateTime(DateTimeEditViewModel vm)
    { return vm.ToDateTime(); }

    public static implicit operator DateTimeEditViewModel(DateTime d)
    { return new DateTimeEditViewModel(d); }


    public virtual IEnumerable<SelectListItem> GetMonthList()
    {
      return months()
        .Select((name, idx) => new SelectListItem {
          Text = name,
          Value = (idx + 1).ToString(),
          Selected = (idx + 1) == Month
        });
    }

    public virtual IEnumerable<SelectListItem> GetDayList()
    {
      return days(Year, Month)
        .Select((day, idx) => new SelectListItem {
          Text = getDayName(idx + 1),
          Value = day.ToString(),
          Selected = (idx + 1) == Day
        });
    }

    public virtual IEnumerable<SelectListItem> GetYearList()
    {
      return years()
        .Select(y => new SelectListItem {
          Text = y.ToString(),
          Value = y.ToString(),
          Selected = Year == y
        });
    }


    private IEnumerable<string> months()
    {
      yield return "January";
      yield return "February";
      yield return "March";
      yield return "April";
      yield return "May";
      yield return "June";
      yield return "July";
      yield return "August";
      yield return "September";
      yield return "October";
      yield return "November";
      yield return "December";
    }

    private IEnumerable<int> days(int year, int month)
    {
      return Enumerable.Range(1, DateTime.DaysInMonth(year, month));
    }

    private IEnumerable<int> years()
    {
      return Enumerable.Range(MinYear, MaxYear - MinYear + 1);
    }


    private string getDayName(int day)
    {
      if (day == 11) return "11 th";
      if (day == 12) return "12 th";
      if (day == 13) return "13 th";

      var lastDigit = day % 10;
      if (lastDigit == 1) return day.ToString() + " st";
      if (lastDigit == 2) return day.ToString() + " nd";
      if (lastDigit == 3) return day.ToString() + " rd";

      return day.ToString() + " th";
    }
  }
}