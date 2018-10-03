using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Script.Serialization;

namespace Reader.Controllers
{
    public class HomeController : Controller
    {
        class AnnotC {
            public int Page;
            public PdfDictionary Annot;
        }
        public ActionResult Index()
        {

            #region [Search Annotations]

            List<AnnotC> Annot = new List<AnnotC>();
            PdfReader vAnnotedReader = new PdfReader(Server.MapPath("~/Data/Extender.pdf"));
            for (int i = 1; i <= vAnnotedReader.NumberOfPages; i++)
            {
                PdfArray array = vAnnotedReader.GetPageN(i).GetAsArray(PdfName.ANNOTS);
                if (array == null) continue;
                //AnnotC c = new AnnotC();
                //c.Page = i;
                //c.Annot = vAnnotedReader.GetPageN(i);
                //Annot.Add(c);
                for (int j = 0; j < array.Size; j++)
                {
                    PdfDictionary annot = array.GetAsDict(j);
                    PdfString text = annot.GetAsString(PdfName.CONTENTS);
                    if (text != null)
                    {
                        AnnotC c = new AnnotC();
                        c.Page = i;
                        c.Annot = annot;
                        Annot.Add(c);
                    }
                }
            }

            PdfReader vAnnotedReader2 = new PdfReader(Server.MapPath("~/Data/Ori.pdf"));
            for (int i = 1; i <= vAnnotedReader2.NumberOfPages; i++)
            {
                PdfArray array = vAnnotedReader2.GetPageN(i).GetAsArray(PdfName.ANNOTS);
                if (array == null) continue;
                for (int j = 0; j < array.Size; j++)
                {
                    PdfDictionary annot = array.GetAsDict(j);
                    PdfString text = annot.GetAsString(PdfName.CONTENTS);
                    if (text != null)
                    {
                        AnnotC c = new AnnotC();
                        c.Page = i;
                        c.Annot = annot;
                        Annot.Add(c);
                    }
                    //PdfAnnotation test = annot;
                }
            }

            //var json = new JavaScriptSerializer().Serialize(Annot);
            PdfReader vOriginalReader = new PdfReader(Server.MapPath("~/Data/Ori.pdf"));
            var pDestinationFileName = Server.MapPath("~/Data/Merge.pdf");
           

            for (int vPageNumber = 1; vPageNumber <= vOriginalReader.NumberOfPages; vPageNumber++)
            {
                PdfDictionary vOriginalPage = vOriginalReader.GetPageN(vPageNumber);


                foreach (var x in Annot)
                {
                    if (x.Page == vPageNumber)
                    {
                        //PdfAnnotation annotation = PdfAnnotation.CreateText(vStamper.Writer, x.Annot.GetAsName(PdfRectangle.ARRAY), "Note", "This document is written by Dara Yuk", false, "Note");

                        PdfString contents = x.Annot.GetAsString(PdfName.CONTENTS);
                        String value = contents.ToString();

                        Rectangle rect = Rectangle()
                        //vOriginalReader.GetPageN(vPageNumber).MergeDifferent(x.Annot);

                        //PdfAnnotation test = x.Annot.GetAsString(PdfName.ann)
                        vOriginalReader.GetPageN(vPageNumber).Put(PdfName.ANNOT,x.Annot);

                        //vOriginalReader.GetPageN(vPageNumber).Merge(x.Annot);

                    }

                }
                //vStamper.MarkUsed(vOriginalPage);
            }



            PdfStamper vStamper = new PdfStamper(vOriginalReader, new FileStream(pDestinationFileName, FileMode.Create));
            vStamper.Close();
            vAnnotedReader.Close();
            vOriginalReader.Close();










            #endregion


            #region [Merging]
            /*//set the output file
            using (Stream outputPdfStream = new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Merge.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //select the PDF files you want to merge, in this example I only merged 2 files, but you can do more.
                string[] pdfArrayFiles = { "C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender.pdf", "C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Ori.pdf" };

                //create a new document
                Document document = new Document();
                //use PdfSmartCopy to merge PDF files to output file (or stream)
                PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream);
                //open the document
                document.Open();
                //set the reader to read files and add pages
                PdfReader reader;
                int n;
                for (int i = 0; i < pdfArrayFiles.Length; i++)
                {
                    reader = new PdfReader(pdfArrayFiles[i]);
                    n = reader.NumberOfPages;
                    for (int page = 0; page < n;)
                    {
                        copy.AddPage(copy.GetImportedPage(reader, ++page));
                    }
                }
                document.Close();
            }*/
            #endregion

            #region [3]

            //Stream sourcePdfStream = new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //Stream destinationPdfStream = new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Ori.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //var output = new MemoryStream();

            //using (var document = new Document(PageSize.A4, 70f, 70f, 20f, 20f))
            //{
            //    var readers = new List<PdfReader>();
            //    var writer = PdfWriter.GetInstance(document, output);
            //    writer.CloseStream = false;
            //    document.Open();
            //    const Int32 requiredWidth = 500;
            //    const Int32 zeroBottom = 647;
            //    const Int32 left = 50;

            //    Action<String, Action> inlcudePdfInDocument = (filename, e) =>
            //    {
            //        var reader = new PdfReader(filename);
            //        readers.Add(reader);

            //        var pageCount = reader.NumberOfPages;
            //        for (var i = 0; i < pageCount; i++)
            //        {
            //            e?.Invoke();
            //            var imp = writer.GetImportedPage(reader, (i + 1));
            //            var scale = requiredWidth / imp.Width;
            //            var height = imp.Height * scale;

            //            writer.DirectContent.AddTemplate(imp, scale, 0, 0, scale, left, zeroBottom - height);

            //            var annots = reader.GetPageN(i + 1).GetAsArray(PdfName.ANNOTS);
            //            if (annots != null && annots.Size != 0)
            //            {
            //                foreach (var a in annots)
            //                {
            //                    var newannot = new PdfAnnotation(writer, new Rectangle(0, 0));
            //                    var annotObj = (PdfDictionary)PdfReader.GetPdfObject(a);
            //                    newannot.PutAll(annotObj);
            //                    var rect = newannot.GetAsArray(PdfName.RECT);
            //                    rect[0] = new PdfNumber(((PdfNumber)rect[0]).DoubleValue * scale + left); // Left
            //                    rect[1] = new PdfNumber(((PdfNumber)rect[1]).DoubleValue * scale); // top
            //                    rect[2] = new PdfNumber(((PdfNumber)rect[2]).DoubleValue * scale + left); // right
            //                    rect[3] = new PdfNumber(((PdfNumber)rect[3]).DoubleValue * scale); // bottom
            //                    writer.AddAnnotation(newannot);
            //                }
            //            }

            //            document.NewPage();
            //        }

            //    };

            //    List<String> pdfs = new List<string>();
            //    pdfs.Add("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender.pdf");
            //    pdfs.Add("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Ori.pdf");
            //    foreach (var apprPdf in pdfs)
            //    {
            //        document.NewPage();
            //        inlcudePdfInDocument(apprPdf, null);
            //    }

            //    document.Close();
            //    readers.ForEach(x => x.Close());

            //}

            //output.Position = 0;
            //FileStream F = new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Merge.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //output.WriteTo(F);
            //F.Close();
            //output.Close();
            #endregion


            #region [Add Annot]
            //path to source file 
            //String source = "C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender.pdf";
            ////create PdfReader object to read the source file
            //PdfReader reader = new PdfReader(source);
            ////create PdfStamper object to modify the content of the PDF
            //PdfStamper stamp = new PdfStamper(reader, new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender - Copy.pdf", FileMode.Create));
            ////create a Rectangle object to deine the size and location of the annotation
            //Rectangle rect = new Rectangle(10, 30, 50, 50);
            ////create a PdfAnnotation object
            //PdfAnnotation annotation = PdfAnnotation.CreateText(stamp.Writer, rect, "Note", "This document is written by Dara Yuk", false, "Note");
            ////loop through the document and add the annotation to every page
            //for (int page = 1; page <= reader.NumberOfPages; page++)
            //    stamp.AddAnnotation(annotation, page);
            //stamp.Close();
            //System.Diagnostics.Process.Start("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender - Copy.pdf");
            //Console.Read();
            #endregion


            #region [Tes Copy Annot]

            //Stream sourcePdfStream = new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Extender.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //Stream destinationPdfStream = new FileStream("C:\\Users\\Athe\\source\\repos\\Reader\\Reader\\Data\\Ori.pdf", FileMode.Create, FileAccess.Write, FileShare.None);

            //// Create new document (IText)
            //Document outdoc = new Document(PageSize.A4);

            //// Seek to Stream start and create Reader for input PDF
            //m.Seek(0, SeekOrigin.Begin);
            //PdfReader inputPdfReader = new PdfReader(sourcePdfStream);

            //// Seek to Stream start and create Reader for destination PDF
            //m.Seek(0, SeekOrigin.Begin);
            //PdfReader destinationPdfReader = new PdfReader(destinationPdfStream);

            //// Create a PdfWriter from for new a pdf destination stream
            //// You should write into a new stream here!
            //Stream processedPdf = new MemoryStream();
            //PdfWriter pdfw = PdfWriter.GetInstance(outdoc, processedPdf);

            //// do not close stream if we've read everything
            //pdfw.CloseStream = false;

            //// Open document
            //outdoc.Open();

            //// get number of pages
            //int numPagesIn = inputPdfReader.NumberOfPages;
            //int numPagesOut = destinationPdfReader.NumberOfPages;

            //int max = numPagesIn;

            //// Process max number of pages
            //if (max < numPagesOut)
            //{
            //    throw new Exception("Impossible - different number of pages");
            //}
            //int i = 0;

            //// Process Pdf pages
            //while (i < max)
            //{
            //    // Import pages from corresponding reader
            //    PdfImportedPage pageIn = writer.inputPdfReader(reader, i);
            //    PdfImportedPage pageOut = writer.destinationPdfReader(reader, i);

            //    // Get named destinations (annotations
            //    List<Annotat ions> toBeAdded = ParseInAndOutAndGetAnnotations(pageIn, pageOut);

            //    // add your annotations
            //    foreach (Annotation anno in toBeAdded) pageOut.Add(anno);

            //    // Add processed page to output PDFWriter
            //    outdoc.Add(pageOut);
            //}

            //// PDF creation finished
            //outdoc.Close();
            #endregion

            #region [Test Copy Annot 2]
            //string oldFile = Server.MapPath("~/Data2/Extender.pdf");
            //string oldFile2 = Server.MapPath("~/Data2/Ori.pdf");
            //string newFile = Server.MapPath("~/Data2/Merge.pdf");
            //// open the reader
            //PdfReader reader = new PdfReader(oldFile);
            //Rectangle size = reader.GetPageSizeWithRotation(1);
            //Document document = new Document(size);

            //PdfReader reader2 = new PdfReader(oldFile2);
            //Rectangle size2 = reader2.GetPageSizeWithRotation(1);
            //Document document2 = new Document(size2);

            //// open the writer

            //// remember to set the page size before opening document
            //// otherwise the page is already set.
            ///* chapter02/HelloWorldMetadata.java */
            //document.Open();

            //// the pdf content
            //// cb does not work with stamper 


            //// create the new pagez and add it to the pdf
            //// this segment of code is meant for writer
            //FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite);

            //PdfStamper writer = new PdfStamper(reader, fs, reader.PdfVersion, false);

            //for (int pg = 1; pg <= reader.NumberOfPages; pg++)
            //{

            //    // taken from http://itextsharp.10939.n7.nabble.com/How-to-edit-annotations-td3352.html

            //    PdfDictionary pagedic = reader.GetPageN(pg);
            //    PdfArray annotarray = (PdfArray)PdfReader.GetPdfObject(pagedic.Get(PdfName.ANNOTS));

            //    if (annotarray == null || annotarray.Size == 0)
            //        continue;
            //    foreach (PdfIndirectReference annot in annotarray.ArrayList)
            //    {
            //        PdfDictionary annotationDic = (PdfDictionary)PdfReader.GetPdfObject(annot);
            //        PdfName subType = (PdfName)annotationDic.Get(PdfName.SUBTYPE);
            //        if (subType.Equals(PdfName.TEXT) || subType.Equals(PdfName.FREETEXT))
            //        {
            //            annotationDic.Put(PdfName.CONTENTS, new PdfString("These are changed contents", PdfObject.TEXT_UNICODE));
            //        }
            //        PdfString contents = annotationDic.GetAsString(PdfName.CONTENTS);
            //        if (contents != null)
            //        {
            //            String value = contents.ToString();
            //            annotationDic.Put(PdfName.CONTENTS, new PdfString(value));
            //            annotationDic.Remove(PdfName.AP);
            //            List<PdfName> tobeDel = new List<PdfName>();
            //            foreach (PdfName key in annotationDic.Keys)
            //            {
            //                if (key.CompareTo(PdfName.AP) == 0 ||
            //                    key.CompareTo(PdfName.RC) == 0 ||
            //                    annotationDic.Get(key).IsDictionary())
            //                {
            //                    tobeDel.Add(key);
            //                }
            //            }
            //            foreach (PdfName key in tobeDel)
            //            {
            //                annotationDic.Remove(key);
            //            }
            //        }
            //        writer.MarkUsed(annotationDic);

            //    }
            //    if ((pg + 1) < reader.NumberOfPages)
            //    {
            //        document.NewPage();
            //    }
            //}


            ////tesstart
            //PdfStamper writer2 = new PdfStamper(reader2, fs, reader2.PdfVersion, false);
            //document2.Open();
            //for (int pg = 1; pg <= reader2.NumberOfPages; pg++)
            //{
            //    PdfDictionary pagedic = reader2.GetPageN(pg);
            //    PdfArray annotarray = (PdfArray)PdfReader.GetPdfObject(pagedic.Get(PdfName.ANNOTS));

            //    if (annotarray == null || annotarray.Size == 0)
            //        continue;
            //    foreach (PdfIndirectReference annot in annotarray.ArrayList)
            //    {
            //        PdfDictionary annotationDic = (PdfDictionary)PdfReader.GetPdfObject(annot);
            //        PdfName subType = (PdfName)annotationDic.Get(PdfName.SUBTYPE);
            //        if (subType.Equals(PdfName.TEXT) || subType.Equals(PdfName.FREETEXT))
            //        {
            //            annotationDic.Put(PdfName.CONTENTS, new PdfString("These are changed contents", PdfObject.TEXT_UNICODE));
            //        }
            //        PdfString contents = annotationDic.GetAsString(PdfName.CONTENTS);
            //        if (contents != null)
            //        {
            //            String value = contents.ToString();
            //            annotationDic.Put(PdfName.CONTENTS, new PdfString(value));
            //            annotationDic.Remove(PdfName.AP);
            //            List<PdfName> tobeDel = new List<PdfName>();
            //            foreach (PdfName key in annotationDic.Keys)
            //            {
            //                if (key.CompareTo(PdfName.AP) == 0 ||
            //                    key.CompareTo(PdfName.RC) == 0 ||
            //                    annotationDic.Get(key).IsDictionary())
            //                {
            //                    tobeDel.Add(key);
            //                }
            //            }
            //            foreach (PdfName key in tobeDel)
            //            {
            //                annotationDic.Remove(key);
            //            }
            //        }
            //        writer2.MarkUsed(annotationDic);

            //    }
            //    if ((pg + 1) < reader2.NumberOfPages)
            //    {
            //        document2.NewPage();
            //    }
            //}
            ////tesend



            //// close the streams and voilá the file should be changed :)

            //writer.Close();
            //reader.Close();
            #endregion

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
