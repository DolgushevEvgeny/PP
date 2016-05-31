@echo off

start Smokers\bin\Debug\Smokers.exe table
start Smokers\bin\Debug\Smokers.exe agent
start Smokers\bin\Debug\Smokers.exe smoker 1
start Smokers\bin\Debug\Smokers.exe smoker 2
start Smokers\bin\Debug\Smokers.exe smoker 3

pause

taskkill /im Smokers.exe /f