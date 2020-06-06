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

$excludeList = "**/Data/Migrations/*%2c**/API/Program.cs"
$testResult = dotnet test /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov /p:ExcludeByFile=$excludeList
$testResult | Out-File .\test-results.txt -Encoding utf8
..\tools\reportgenerator.exe -reports:.\TestResults\coverage.info -targetdir:.\TestResults\
start .\TestResults\index.htm