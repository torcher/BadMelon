$reportGeneratorPath = "..\tools\reportgenerator.exe"
$testResultsPath = ".\TestResults"

if(-not(Test-Path $reportGeneratorPath)){
    Set-Location ..
    dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
    Set-Location .\tests
}

if(Test-Path $testResultsPath){
    Remove-Item $testResultsPath -R -Force
}

$testResults = dotnet test /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov
$t = $testResults.IndexOf("+--")
$testResults | Out-File .\test-results.txt

..\tools\reportgenerator.exe -reports:.\TestResults\coverage.info -targetdir:.\TestResults\
start .\TestResults\index.htm