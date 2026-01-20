# 1. Giai đoạn Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ code vào image
COPY . .

# Restore và Build (Chỉ định rõ project chạy chính là API)
WORKDIR "/src/QLKTX_API"
RUN dotnet restore "QLKTX_API.csproj"
RUN dotnet publish "QLKTX_API.csproj" -c Release -o /app/publish

# 2. Giai đoạn Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Cấu hình port cho Render
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "QLKTX_API.dll"]
