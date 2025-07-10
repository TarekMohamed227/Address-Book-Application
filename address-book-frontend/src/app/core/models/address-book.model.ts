export interface AddressBookEntry {
  Id?: number | null;
  FullName: string;
  Email: string;
  Password?: string;
  MobileNumber: string;
  DateOfBirth: string;
  Address: string;
  JobTitleId: number;
  DepartmentId: number;
  PhotoUrl?: string;  

  Department?: {
    Name: string;
  };

  JobTitle?: {
    Name: string;
  };
}

export interface UpdateAddressBookEntryDto {
  id: number;
  fullName: string;
  email: string;
  password: string;
  mobileNumber: string;
  dateOfBirth: Date;
  address: string;
  jobTitleId: number;
  departmentId: number;
  photo?: File;  
}


