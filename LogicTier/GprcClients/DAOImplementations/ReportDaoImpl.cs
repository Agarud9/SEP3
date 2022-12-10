﻿using Application.DAOInterfaces;
using GprcClients.Converters;

namespace GprcClients.DAOImplementations;

public class ReportDaoImpl : IReportDao
{
    private ReportService.ReportServiceClient client;
    private IImageDao imageDao;

    public ReportDaoImpl(ReportService.ReportServiceClient client, IImageDao imageDao)
    {
        this.client = client;
        this.imageDao = imageDao;
    }
    
    public async Task<ICollection<Shared.Models.Report>> GetAllReportsAsync()
    {
        Reports grpcReports = await client.GetReportsAsync(new Void());
        ICollection<Shared.Models.Report> list = new List<Shared.Models.Report>();

        foreach (var report in grpcReports.Reports_)
        {
            if (report is null)
            {
                continue;
            }

            Shared.Models.Report reportToSend = report.ToShared(imageDao);
            list.Add(reportToSend);
        }
        return list;
    }

    public async Task DeleteReportAsync(long id)
    {
        try
        {
            await client.DeleteReportAsync(id.ToGrpc());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Shared.Models.Report> CreateReportAsync(Shared.Models.Report report)
    {
        var grpcReport = await client.CreateReportAsync(report.ToGrpc());
        return grpcReport.ToShared(imageDao);
    }
}