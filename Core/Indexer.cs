using Search_Engine_Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Search_Engine_Project.Models;
using Search_Engine_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Search_Engine_Project.Core;

namespace Search_Engine_Project.Core
{
    public class Indexer
    {

        private readonly WordService _WordService;

        public Indexer(WordService wordService)
        {
            _WordService = wordService;
        }

        public bool indexFile(string file)
        {   
            try
            {
                
                Semanter semanter = new Semanter(file);
                Dictionary<string, int> scannedResults = semanter.getScannedDocumentData();
                foreach(KeyValuePair<string, int> entry in scannedResults)
                {
                    string word = entry.Key;
                    int frequency = entry.Value;

                    Word wordDocument = _WordService.FindByKeyword(word);

                   if (wordDocument == null) {
                        Console.WriteLine("Null -- " + word);
                        // key value pair for the document file name and the frequency of the current word in it.
                        var documents = new Dictionary<string, int>();
                        documents.Add(file, frequency);
                        Word _wordDocument = new Word();
                        _wordDocument.Value = word;
                        _wordDocument.Documents = documents;
                        Word data = _WordService.Create(_wordDocument);
                        Console.WriteLine("created " + data.Value + " " + data.Id);
                   }
                    else
                    {
                        // if keyword already exists, update it with a new file document
                        if (!wordDocument.Documents.ContainsKey(file))
                        {
                            Console.WriteLine("Doesn't Existss");
                            wordDocument.Documents.Add(file, frequency);
                            _WordService.Update(wordDocument.Id, wordDocument);
                        }
                    }
                }

                Console.WriteLine("New document indexing completed! successfully");
                return true;
            } catch (Exception Ex)
            {   
                Console.WriteLine("Error while indexing document, " +  Ex);
                return false;
            }

        }
    }
}