using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;

namespace eolinkerToWord
{
    class Program
    {
        #region test
        //static void Main(string[] args)
        //{

        //    //载入Word文档
        //    Document document = new Document("./doc/API文档.docx");

        //    foreach (ITable table in document.Sections[0].Tables)
        //    {
        //        var t = table;
        //        //t.TableFormat.Borders.Horizontal.BorderType = BorderStyle.Hairline;
        //        //t.TableFormat.Borders.Horizontal.Color = Color.Orange;
        //        //t.TableFormat.Borders.Vertical.BorderType = BorderStyle.Hairline;
        //        //t.TableFormat.Borders.Vertical.Color = Color.Orange;


        //        Console.WriteLine("请求URL：" + t.Rows[1].Cells[1].Paragraphs[0].Text + "----" + t.TableFormat.Borders.Vertical.Color);
        //        t.TableFormat.Borders.LineWidth = 2.0F;
        //        t.TableFormat.Borders.BorderType = BorderStyle.Double;
        //        t.TableFormat.Borders.Vertical.Color = Color.Black;
        //        Console.WriteLine("请求URL：" + t.Rows[1].Cells[1].Paragraphs[0].Text + "----" + t.TableFormat.Borders.Vertical.Color);
        //    }


        //    Console.WriteLine("请求URL：" + document.Sections[0].Tables[3].Rows[1].Cells[1].Paragraphs[0].Text + "----" + document.Sections[0].Tables[3].TableFormat.Borders.Vertical.Color);
        //    document.SaveToFile($"./doc/API文档---{DateTime.Now:yyyyMMddHHmmss}.docx", FileFormat.Docx);
        //    Console.ReadKey();
        //}
        #endregion

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //地区数据
            string jsonStr = File.ReadAllText("./doc/eoapi.json");
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<eoapi>(jsonStr);


            #region 将数据插入文档

            //载入Word文档
            Document document = new Document("./doc/API模板.docx");

            model.ApiGroupList.ForEach(u =>
            {
                //一级标题
                var pTitle1 = document.Sections[0].Paragraphs[0] as Paragraph;
                var pTitle1_new = pTitle1.Clone() as Paragraph;
                pTitle1_new.Text = u?.GroupName.Replace("&gt;", ">");
                document.Sections[0].Paragraphs.Add(pTitle1_new);
                var apiIndex = 1;
                u.ApiList.ForEach(o =>
                {
                    //二级标题
                    var pTitle2 = document.Sections[0].Paragraphs[1] as Paragraph;
                    var pTitle2_new = pTitle2.Clone() as Paragraph;
                    pTitle2_new.Text = apiIndex++ + "、" + o.BaseInfo.apiName.Replace("&gt;", ">");
                    document.Sections[0].Paragraphs.Add(pTitle2_new);

                    //API参数表格
                    Table table = document.Sections[0].Tables[0] as Table;
                    var tableNew = table.Clone();
                    document.Sections[0].Tables.Add(tableNew);

                    //表格样式，设置了但是没有起作用，原因未知；
                    tableNew.TableFormat.Borders.LineWidth = 1.0F;
                    tableNew.TableFormat.Borders.BorderType = BorderStyle.Double;
                    tableNew.TableFormat.Borders.Color = Color.Black;

                    var tableNew1 = document.Sections[0].Tables[apiIndex - 1];
                    tableNew1.TableFormat.Borders.Horizontal.BorderType = BorderStyle.Hairline;
                    tableNew1.TableFormat.Borders.Horizontal.Color = Color.Orange;
                    tableNew1.TableFormat.Borders.Vertical.BorderType = BorderStyle.Hairline;
                    tableNew1.TableFormat.Borders.Vertical.Color = Color.Orange;

                    //向表格中添加数据

                    //请求类型
                    tableNew.Rows[0].Cells[1].Paragraphs[0].Text = o.BaseInfo.apiRequestType;
                    //请求URL
                    tableNew.Rows[1].Cells[1].Paragraphs[0].Text = o.BaseInfo.apiURI.Replace("&amp;", "&");

                    //请求参数
                    var rowIndex = 0;//请求参数行序号
                    o.RequestInfo.ForEach(r1 =>
                    {
                        var newRow = tableNew.Rows[rowIndex + 4];
                        newRow.Cells[0].Paragraphs[0].Text = r1.paramKey;
                        newRow.Cells[1].Paragraphs[0].Text = r1.paramName;
                        newRow.Cells[2].Paragraphs[0].Text = r1.paramType;
                        newRow.Cells[3].Paragraphs[0].Text = r1.paramNote;
                        newRow.Cells[4].Paragraphs[0].Text = r1.paramNotNull;

                        var newRow1 = tableNew.Rows[rowIndex + 4].Clone();
                        tableNew.Rows.Insert(rowIndex + 5, newRow1);
                        rowIndex++;
                    });
                    if (rowIndex > 0)
                    {
                        tableNew.Rows.RemoveAt(rowIndex + 4);
                    }

                    if (rowIndex == 0)
                    {
                        rowIndex += 1;
                    }

                    //返回参数
                    var rowIndex1 = 0;
                    o.ResultInfo.ForEach(r2 =>
                    {
                        //对于返回的前两个字段code，msg不处理
                        if (rowIndex1 > 2)
                        {
                            var newRow = tableNew.Rows[rowIndex + 6 + rowIndex1];
                            newRow.Cells[1].Paragraphs[0].Text = r2.paramKey;
                            newRow.Cells[2].Paragraphs[0].Text = r2.paramName;
                            var value = r2.paramValueList.Count > 0 ? r2.paramValueList[0]?.value : "";
                            //返回参数类型靠猜
                            var retType = "object";
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (Regex.IsMatch(value, "^\\d*$"))
                                {
                                    retType = "int";
                                }
                                else if (Regex.IsMatch(value, "^\\d*\\.\\d*$"))
                                {
                                    retType = "decimal";
                                }
                                else
                                {
                                    retType = "string";
                                }

                            }
                            newRow.Cells[3].Paragraphs[0].Text = retType;
                            newRow.Cells[4].Paragraphs[0].Text = value;
                            newRow.Cells[5].Paragraphs[0].Text = r2.paramNotNull;

                            var newRow1 = tableNew.Rows[rowIndex + rowIndex1 + 6].Clone();
                            tableNew.Rows.Insert(rowIndex + rowIndex1 + 7, newRow1);
                        }
                        else if (rowIndex1 == 2)
                        {
                            var newRow = tableNew.Rows[rowIndex + 8];
                            newRow.Cells[1].Paragraphs[0].Text = r2.paramName;
                            var value = r2.paramValueList.Count > 0 ? r2.paramValueList[0]?.value : "";
                            newRow.Cells[3].Paragraphs[0].Text = value;
                        }
                        rowIndex1++;
                    });
                    if (rowIndex1 > 3)
                    {
                        tableNew.Rows.RemoveAt(rowIndex + rowIndex1 + 6);
                    }
                });

            });

            document.SaveToFile($"./doc/API文档-{DateTime.Now:yyyyMMddHHmmss}.docx", FileFormat.Docx);

            #endregion

            sw.Stop();
            Console.WriteLine("耗时：" + sw.Elapsed);
            Console.ReadKey();
        }


    }
}
