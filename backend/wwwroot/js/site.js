'use strict';

class Signal {
    constructor() {
        this.connection = (new signalR.HubConnectionBuilder()).withUrl('/ReportsHub').build();
        this.connection.on('NewReportAsync', async (i, p, m, a, t, o) => await this.onNewReport(i, { product: p, message: m, attachment: a, timestamp: t, open: o }));
        this.connection.on('ReportResolvingAsync', async i => await this.onReportOpened(i));
        this.connection.on('ReportReopenedAsync', async i => await this.onReportClosed(i));
        this.connection.on('ReportResolvedAsync', (i, n, p, m, a, t, o) => this.onReportResolved(i, n, { product: p, message: m, attachment: a, timestamp: t, open: o }));
        this.connection.on('StartupAsync', async (l, w, r) => await this.onStartup(l, w, r));
        this.connection.start();
    }

    // Abstracts
    async onNewReport(reportId, reportData) { }
    async onReportOpened(reportId) { }
    async onReportClosed(reportId) { }
    async onReportResolved(reportId, nextReportId, nextReportData) { }
    async onStartup(reports, waitingCount, resolvingCount) { }
}