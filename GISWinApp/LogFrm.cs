using GISCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISWinApp
{
    public partial class LogFrm : Form
    {
        private readonly DateTime _dtStart;
        private readonly DateTime _dtEnd;

        public LogFrm(DateTime dtStart, DateTime dtEnd)
        {
            InitializeComponent();

            _dtEnd = dtEnd;
            _dtStart = dtStart;
        }

        /// <summary>
        /// 显示历史数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        private void ShowLog(DateTime dtStart, DateTime dtEnd)
        {
            using (GISContext context = new GISContext())
            {
                var logs = context.Infoes.Where(info => info.Time >= dtStart && info.Time <= dtEnd).OrderByDescending(x => x.Id);

                if (logs == null || logs.Count() == 0)
                {
                    MessageBox.Show("没有该数据!");

                    this.Close();
                }

                dataLogs.Rows.Clear();

                foreach (var log in logs)
                {
                    var row = new DataGridViewRow();

                    row.CreateCells(dataLogs);

                    row.Cells[0].Value = log.Temp;
                    row.Cells[1].Value = log.NH3;
                    row.Cells[2].Value = log.Light;
                    row.Cells[3].Value = log.Hum;
                    row.Cells[4].Value = log.Time;

                    dataLogs.Rows.Add(row);
                }
            }
        }

        private void LogFrm_Load(object sender, EventArgs e)
        {
            this.ShowLog(_dtStart, _dtEnd);
        }
    }
}
