using System.Windows.Forms;
using System;
using System.Collections;

namespace HSp.Worktime
{
public class AccountingListDialog : MonthListDialog
{
	public AccountingListDialog(Form owner, DateTime start, DateTime end, string titel) : base(owner, start, end, titel)
	{
	}

	protected override void FillWithData()
	{
		Project project;
		ArrayList workdayProjectLinks;
		string[] strArrays;
		string[] strArrays2 = null;
		string[] strArrays3 = null;
		WorkDay workDay1 = MainApp.workDayVerwo;
		WorkDayProjectLink workDayProjectLink1 = MainApp.workDayProjectLinkVerwo;
		string[] strArrays4 = new string[4];
		TimeSpan timeSpan1 = TimeSpan.FromHours(MainApp.NominalWorkTime);
		TimeSpan timeSpan2 = new TimeSpan(0, 0, 0);
		Hashtable hashtables = new Hashtable();
		Hashtable key = new Hashtable();
		Hashtable str1 = new Hashtable();
		string str2 = string.Concat(this.dtStart.ToString("yyyy-MM-dd"), " 00:00:00");
		string str3 = string.Concat(this.dtEnd.ToString("yyyy-MM-dd"), " 23:59:59");
		string[] str4 = new string[5];
		
        str4[0] = "Kontierungen von ";
		str4[1] = this.dtStart.ToString("dd.MM.yyyy");
		str4[2] = " bis ";
		str4[3] = this.dtEnd.ToString("dd.MM.yyyy");
		str4[4] = "\r\n\r\n";
		string str5 = string.Concat(str4);
		
        ArrayList workDays = workDay1.Select(string.Concat(new string[] { "[DayDate] >= #", str2, "# AND [DayDate] <= #", str3, "#" }), "[DayDate]");

		if (workDays == null)
		    return;
		
		foreach (WorkDay workDay2 in workDays)
		{
			workdayProjectLinks = workDayProjectLink1.Select(string.Concat("[WorkDayOid] = '", workDay2.oid, "'"));
			if (workdayProjectLinks == null)
				break;
			
			foreach (WorkDayProjectLink workDayProjectLink2 in workdayProjectLinks)
			{
				project = workDayProjectLink2.ResolveOid(workDayProjectLink2.ProjectOid) as Project;
				if (project != null)
				{
					hashtables.Item = project.oid.ToString();
					str1.Item = project.oid.ToString();
				}
			}
		}

		str5 = string.Concat(str5, "    Datum  ");
		string str6 = "-----------";
		
        IDictionaryEnumerator enumerator = hashtables.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string @value = enumerator.Value as string;
			string[] strArrays5 = this.SplitString(@value, 12);
			for (int i = 0; i < (int)strArrays4.Length; i++)
			{
				if (i < (int)strArrays5.Length)
				{
					i[IntPtr intPtr = i] = string.Concat(strArrays2[intPtr], "| ", strArrays5[i].PadRight(12), " ");
					continue;
				}
				i[IntPtr intPtr2 = i] = string.Concat(strArrays3[intPtr2], "| ", new string(32, 12), " ");
			}
			str6 = string.Concat(str6, "+-", new string(45, 13));
		}
		for (int j = 0; j < 4; j++)
		{
			if (j > 0)
			{
				str5 = string.Concat(str5, new string(32, 11));
			}
			str5 = string.Concat(str5, strArrays4[j], "\r\n");
		}
		str5 = string.Concat(str5, str6);
		str5 = string.Concat(str5, "\r\n");
		foreach (WorkDay workDay3 in workDays)
		{
			DateTime outTime = workDay3.OutTime;
			TimeSpan timeSpan3 = outTime.Subtract(workDay3.InTime);
			TimeSpan timeSpan4 = timeSpan3.Subtract(workDay3.PauseTime);
			timeSpan2 = timeSpan2.Add(timeSpan4);
			enumerator = hashtables.GetEnumerator();
			
            while (enumerator.MoveNext())
			{
				key.Item = enumerator.Key as string;
			}
			
            workdayProjectLinks = workDayProjectLink1.Select(string.Concat("[WorkDayOid] = '", workDay3.oid, "'"));
			if (workdayProjectLinks == null)
			{
				break;
			}
			
            foreach (WorkDayProjectLink workDayProjectLink3 in workdayProjectLinks)
			{
				project = workDayProjectLink3.ResolveOid(workDayProjectLink3.ProjectOid) as Project;
				if (workDayProjectLink3.ProjectOid != null)
				{
					double percentage = workDayProjectLink3.Percentage / 100 * timeSpan4.TotalHours;
					key.Item = workDayProjectLink3.ProjectOid.ToString();
					str1.Item = workDayProjectLink3.ProjectOid.ToString();
				}
			}

			str5 = string.Concat(workDay3.DayDate.ToString("dd.MM.yyyy"), " ");
			
            enumerator = hashtables.GetEnumerator();
			while (enumerator.MoveNext())
			{
				str5 = string.Concat(str5, string.Format("| {0,12:f2} ", (double)key[enumerator.Key]));
			}
			str5 = string.Concat(str5, "\r\n");
		}
		str5 = string.Concat(str5, str6);
		str5 = string.Concat(str5, "\r\n");
		str5 = string.Concat(str5, "     Summe ");
		
        enumerator = hashtables.GetEnumerator();
		
        while (enumerator.MoveNext())
		{
			str5 = string.Concat(str5, string.Format("| {0,12:f2} ", (double)str1[enumerator.Key]));
		}
		
        str5 = string.Concat(str5, "\r\n", str6);
		str5 = "Keine Eintragungen im Datumsbereich";
		
        this.infoTB.Text = str5;
		this.infoTB.SelectionStart = 200000;
	}

	protected string[] SplitString(string str, int plen)
	{
		ArrayList arrayLists = new ArrayList();
		string str2 = str;
		while (str2.Length > plen)
		{
			string str3 = str2.Substring(0, plen);
			str2 = str2.Substring(plen);
			arrayLists.Add(str3);
		}
		if (str2.Length > 0)
		{
			arrayLists.Add(str2);
		}
		string[] item = new string[arrayLists.Count];
		for (int i = 0; i < arrayLists.Count; i++)
		{
			item[i] = (string)arrayLists[i];
		}
		return item;
	}
}
}