﻿using Shared.DTOs;
using Shared.Models;

namespace HttpClient.ClientInterfaces;

public interface IReportService
{
    public Task<ICollection<Report>> GetAllReports();
    Task IgnoreReportAsync(long id);

    Task NotifyFarmerAsync(long id);
    Task ReportOfferAsync(ReportCreationDto dto);
}