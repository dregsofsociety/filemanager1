using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FailMen
{
    public partial class Form1 : Form
    {
        private string filePath = "D:";
        private bool isFile = false;
        private string currentySelectedItemName = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FilePathTextBox.Text = filePath;
            loadFilesAndDerectories();
        }
        public void loadFilesAndDerectories() 
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAtt;

            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "/" + currentySelectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileNameLable.Text = fileDetails.Name;
                    fileTipeLable.Text = fileDetails.Extension;
                    fileAtt = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileAtt = File.GetAttributes(filePath);
                    
                }
                if ((fileAtt & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles(); 
                    DirectoryInfo[] dirs = fileList.GetDirectories(); 

                    string fileExtension = "";
                    listView1.Items.Clear();
                    for (int i = 0; i < files.Length; i++)
                    {
                        fileExtension = files[i].Extension.ToUpper();
                        switch(fileExtension)
                        {
                            case ".MP3":
                            case ".MP2":
                                listView1.Items.Add(files[i].Name, 4);
                                break;
                            case ".EXE":
                            case ".COM":
                                listView1.Items.Add(files[i].Name, 6);
                                break;
                            case ".MP4":
                            case ".AVI":
                            case ".MKV":
                                listView1.Items.Add(files[i].Name, 5);
                                break;
                            case ".PDF":
                                listView1.Items.Add(files[i].Name, 3);
                                break;
                            case ".DOC":
                            case ".DOCX":
                                listView1.Items.Add(files[i].Name, 2);
                                break;
                            case ".PNG":
                            case ".JPG":
                            case ".JPEG":
                                listView1.Items.Add(files[i].Name, 7);
                                break;
                            default:
                                listView1.Items.Add(files[i].Name, 8);
                                break;

                        }
                        listView1.Items.Add(files[i].Name, 8);
                    }
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name, 0);
                    }
                }
                else
                {
                    fileNameLable.Text = this.currentySelectedItemName;
                }

            }

            catch(Exception e)
            {

            }
        }


        public void loudButtomAction()
        {
            removeBackSlash();
            filePath = FilePathTextBox.Text;
            loadFilesAndDerectories();
            isFile = false;
        }

        public void removeBackSlash()
        {
            string path = FilePathTextBox.Text;
            if (path.LastIndexOf("/") == path.Length - 1)
            {
                FilePathTextBox.Text = path.Substring(0, path.Length - 1); 
            }
        }

        public void goBack()
        {
            try
            {
                removeBackSlash();
                string path = FilePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this.isFile = false;
                FilePathTextBox.Text = path;
                removeBackSlash();
            }
            catch (Exception e)
            {
                
            }
            
        }

        private void GoButtom_Click(object sender, EventArgs e)
        {
            loudButtomAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentySelectedItemName = e.Item.Text;

            FileAttributes fileAtt = File.GetAttributes(filePath + "/" + currentySelectedItemName);
            if ((fileAtt & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = false;
                FilePathTextBox.Text = filePath + "/" + currentySelectedItemName;
            }
            else
            {
                isFile = true;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loudButtomAction();
        }

        private void BackButtom_Click(object sender, EventArgs e)
        {
            goBack();
            loudButtomAction();
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 addform = new Form2();
                addform.filePath = filePath;
                addform.Show();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void CreateDirectoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                Form3 addform = new Form3();
                addform.filePath = filePath;
                addform.Show();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void DelButton_Click(object sender, EventArgs e)
        {

            try
            {
                FileAttributes fileAttributes = File.GetAttributes(filePath + "/" + currentySelectedItemName);
                if (fileAttributes.ToString().IndexOf("Directory") != -1)
                        Directory.Delete(filePath + "/" + currentySelectedItemName);
                else
                        File.Delete(filePath + "/" + currentySelectedItemName);
                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }

            }
        
        private void copyDir(string copiedFile, string finishedFile, bool recursive)//Копирование папки
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(copiedFile);
                Directory.CreateDirectory(finishedFile);
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(finishedFile, file.Name);
                    file.CopyTo(targetFilePath);
                }
                if (recursive)
                {
                    foreach (DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = Path.Combine(finishedFile, subDir.Name);
                        copyDir(subDir.FullName, newDestinationDir, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        public void copy()//Копирование файла или директории
        {
            try
            {
                FolderBrowserDialog browse = new FolderBrowserDialog();
                browse.ShowDialog();
                FileAttributes fileAttributes = File.GetAttributes(filePath + "\\" + currentySelectedItemName);
                if (fileAttributes.ToString().IndexOf("Directory") != -1)
                {
                    copyDir(filePath + "\\" + currentySelectedItemName, browse.SelectedPath + "\\" + currentySelectedItemName, true);
                }
                else
                    File.Copy(filePath + "\\" + currentySelectedItemName, browse.SelectedPath + "\\" + currentySelectedItemName);
                MessageBox.Show("Копирование прошло успешно!", "Завершено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            copy();
        }
    }
    }
 
    

