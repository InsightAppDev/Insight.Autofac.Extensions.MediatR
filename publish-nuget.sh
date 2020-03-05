cd ./src

dotnet build -c Release

dotnet pack *.csproj -c Release --output "."

dotnet nuget push -s ${1} -k ${2} "Insight.Autofac.Extensions.MediatR.${3}.nupkg"

cd ..
