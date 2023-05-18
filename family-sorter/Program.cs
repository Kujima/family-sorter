using family_sorter;

var pathExcel = @"C:\Dev\Lab\Dot-net\input\modelisation-base.csv";

Worker worker = new Worker(pathExcel);
worker.Execute();
