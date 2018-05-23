using GISCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISWinApp
{
    public partial class InitFrm : Form
    {
        private int _curCount = 1;

        public InitFrm()
        {
            InitializeComponent();
        }

        private void InitFrm_Shown(object sender, EventArgs e)
        {
            if (!LogHelper.WebIsOk) this.InitWeb();

            if (!LogHelper.DbIsOk) this.InitDB();

            if (LogHelper.WebIsOk && LogHelper.DbIsOk) this.AutoCloseFrm();
        }

        /// <summary>
        /// 初始化 Web
        /// </summary>
        private void InitWeb()
        {
            var webroot = Path.Combine(AppContext.BaseDirectory, GISConst.WebRoot);

            MessageBox.Show("请选择 Web 相关文件, 应该是安装包下的 Web 目录.", "提示");

            FolderBrowserDialog folder = new FolderBrowserDialog();

            folder.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folder.SelectedPath))
            {
                if (File.Exists(Path.Combine(folder.SelectedPath, GISConst.WebAppName)))
                {
                    var count = FileHepler.CountFile(folder.SelectedPath);

                    void logger(string fileName)
                    {
                        proBarCopy.Value = (int)((_curCount++ / (double)count) * 100);

                        this.ShowMsg("正在复制: " + fileName);
                    }

                    FileHepler.CopyDir(folder.SelectedPath, webroot, logger);

                    LogHelper.CopyWebOk();

                    this.ShowMsg("Web 文件复制完成！");

                    return;
                }
            }

            this.ShowMsg("路径错误, 找不到网站文件!请重新选择.");

            var result = MessageBox.Show("路径错误, 找不到网站文件! 点击 确定 重新选择； 点击 取消 退出.", "路径错误", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                this.InitWeb();
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void InitDB()
        {
            using (GISContext context = new GISContext())
            {
                this.ShowMsg("正在迁移数据库,请稍后...");

                // 迁移数据库
                context.Database.Migrate();

                try
                {
                    // 初始化数据
                    if (!context.Users.Any())
                    {
                        List<User> users = new List<User>();

                        for (int i = 0; i < 9; i++)
                        {
                            users.Add(new User { UserName = "00" + (i + 1), Password = "123456".Encryption("00" + (i + 1)) });
                        }

                        context.Users.AddRange(users);
                        context.SaveChanges();
                    }

                    this.ShowMsg("数据库迁移成功!");

                    LogHelper.MigrateDbOk();
                }
                catch (Exception e)
                {
                    this.ShowMsg("数据库迁移失败! " + e.Message);

                    this.ShowMsg("遇到大麻烦了。。。。。。。。。");
                }
            }
        }

        /// <summary>
        /// 自动关闭页面
        /// </summary>
        public void AutoCloseFrm()
        {
            this.ShowMsg(Environment.NewLine + "现在一切准备就绪，您可以关闭配置页面了.");

            int time = 5;

            while (time > 0)
            {
                this.ShowMsg($"页面将在 {time}s 后自动关闭!");
                time--;
                Thread.Sleep(1000);
            }

            this.Close();
        }

        /// <summary>
        /// 打印消息
        /// </summary>
        /// <param name="msg">消息</param>
        private void ShowMsg(string msg)
        {
            txtMsg.AppendText(msg + Environment.NewLine);
        }
    }
}
