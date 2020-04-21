'use strict';

class Index extends Signal {
    constructor() {
        super();
        this.body = $('.card-center ul.mdl-list');
        this.template = this.body.find('.js-template');
        this.waiting = 0;
        this.resolving = 0;
    }
    onNewReport(reportId, reportData) {
        this.addWaiting();
        this.getChildCount() < 10 && this.addElementFor(reportId, reportData.product, reportData.message, reportData.attachment, reportData.timestamp, reportData.open);
        this.updateShowingCount();
    }
    onReportOpened(reportId) {
        this.addResolving();
        const elmt = this.body.find('#report-entity-' + reportId);
        elmt.find('.js-disable').attr('disabled', 'disabled');
        elmt.find('.mdl-list__item-avatar').removeClass('mdl-color--accent');
        elmt.find('a.js-disable').each(function () {
            $(this).data('link-href', $(this).attr('href'));
            $(this).removeAttr('href');
        });
        elmt.find('.js-gray').addClass('mdl-color-text--grey');
    }
    onReportClosed(reportId) {
        this.removeResolving();
        const elmt = this.body.find('#report-entity-' + reportId);
        elmt.find('.js-disable').removeAttr('disabled');
        elmt.find('.mdl-list__item-avatar').addClass('mdl-color--accent');
        elmt.find('a.js-disable').each(function () {
            $(this).attr('href', $(this).data('link-href'));
            $(this).removeData('link-href');
        });
        elmt.find('.js-gray').removeClass('mdl-color-text--grey');
    }
    onReportResolved(reportId, nextReportId, nextReportData) {
        this.addResolved();
        this.body.find('#report-entity-' + reportId).slideUp('fast').delay(500).remove();
        nextReportId && nextReportData && this.addElementFor(nextReportId, nextReportData.product, nextReportData.message, nextReportData.attachment, nextReportData.timestamp, nextReportData.open);
        this.smartToggleEmpty();
    }
    onStartup(reports, w, r) {
        this.addResolving(r);
        this.addWaiting(w);
        $('.js-loading').slideUp('fast');
        reports.length > 0 && reports.forEach(r => this.addElementFor(r.reportId, r.productName, r.message, r.hasAttachment, r.timestamp, r.open));
        this.smartToggleEmpty();
        this.updateShowingCount();
    }
    getChildCount() {
        return this.body.find('.js-child').length;
    }
    addElementFor(reportId, productName, message, hasAttachment, timestamp, open) {
        const elmt = this.template.clone(true).addClass('js-child').removeClass('js-template').data('entity-id', reportId).attr('id', 'report-entity-' + reportId).hide();
        elmt.find('.js-template__code').text('#' + reportId.toString().padStart(6, '0'));
        elmt.find('.js-template__product').text(productName);
        elmt.find('.js-template__message').text(message.length > 150 ? message.substring(0, 150) + '...' : message);
        elmt.find('.js-template__timestamp').text(new Date(timestamp).toLocaleString());
        console.debug(hasAttachment);
        hasAttachment || elmt.find('.js-template__attachment').hide();
        elmt.find('.js-template__link').attr('href', '/Resolve?id=' + reportId);
        if (!open) {
            elmt.find('.js-disable').attr('disabled', 'disabled');
            elmt.find('.js-gray').addClass('mdl-color-text--grey');
            elmt.find('.mdl-list__item-avatar').removeClass('mdl-color--accent');
            elmt.find('a.js-disable').each(function () {
                $(this).data('link-href', $(this).attr('href'));
                $(this).removeAttr('href');
            });
        }
        this.body.append(elmt);
        elmt.slideDown('fast');
    }
    addWaiting(count = 1) {
        this.waiting += count;
        $('.js-waiting').text(this.waiting);
    }
    addResolving(count = 1) {
        this.waiting -= count;
        this.waiting < 0 && (this.waiting = 0);
        this.resolving += count;
        $('.js-waiting').text(this.waiting);
        $('.js-resolving').text(this.resolving);
    }
    removeResolving(count = 1) {
        this.waiting += count;
        this.resolving -= count;
        this.resolving < 0 && (this.resolving  = 0);
        $('.js-waiting').text(this.waiting);
        $('.js-resolving').text(this.resolving);
    }
    addResolved(count = 1) {
        this.resolving -= count;
        this.resolving < 0 && (this.resolving = 0);
        $('.js-resolving').text(this.resolving);
        this.updateShowingCount();
    }
    smartToggleEmpty() {
        if (this.getChildCount() > 0) this.body.find('.js-empty').slideUp('fast');
        else this.body.find('.js-empty').slideDown('fast');
    }
    updateShowingCount() {
        $('.js-showing').text(this.getChildCount() + ' of ' + (this.resolving + this.waiting));
    }
}

const Page = new Index();
