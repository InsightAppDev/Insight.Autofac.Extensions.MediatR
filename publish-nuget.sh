package_name="Insight.Autofac.Extensions.MediatR"

cd "src/${package_name}"

dotnet build -c Release

dotnet pack *.csproj -c Release --output "."

dotnet nuget push -s ${1} -k ${2} "${package_name}.${3}.nupkg"