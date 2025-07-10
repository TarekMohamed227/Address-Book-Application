
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { Department } from '../models/department.model';
import { JobTitle } from '../models/job-title.model';



export const exportToExcel = (data: any[], jobTitles: JobTitle[], departments: Department[], fileName: string): void => {

  const filteredData = data.map(entry => {
   
    const { Id,PhotoUrl, JobTitleId, DepartmentId, ...rest } = entry;

    const jobTitle = jobTitles.find(job => job.id === JobTitleId)?.name || 'N/A';

    const department = departments.find(dep => dep.Id === DepartmentId)?.Name || 'N/A';

    return {
      ...rest,
      JobTitle: jobTitle,
      Department: department,
    };
  });

  const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(filteredData);
  const workbook: XLSX.WorkBook = { Sheets: { data: worksheet }, SheetNames: ['data'] };

  const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

  const blob: Blob = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

  FileSaver.saveAs(blob, `${fileName}.xlsx`);
};
