using family_sorter;

var pathExcel = @"C:\Dev\Lab\Dot-net\input\testpourapp.csv";

Worker worker = new Worker(pathExcel);
worker.Execute();
