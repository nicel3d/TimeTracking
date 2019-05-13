export enum GroupEmitEnum {
  ADD_GROUP = 'ADD_GROUP',
  EDIT_GROUP = 'EDIT_GROUP',

  CHANGE_GROUP_SUCCESS = 'CHANGE_GROUP_SUCCESS',
}

export enum ApplicationGroupEmitEnum {
  ADD_APPLICATION_GROUP = 'ADD_APPLICATION_GROUP',
  EDIT_APPLICATION_GROUP = 'EDIT_APPLICATION_GROUP',
  CHANGE_APPLICATION_GROUP_SUCCESS = 'CHANGE_APPLICATION_GROUP_SUCCESS',
}

export enum StaffEmitEnum {
  EDIT_STAFF = 'EDIT_STAFF',
  CHANGE_STAFF_SUCCESS = 'CHANGE_STAFF_SUCCESS',
}

export interface IApplicationsIdsAndGroupId {
  groupId: number;
  applicationsIds: number[];
}

export class ApplicationsIdsAndGroupId implements IApplicationsIdsAndGroupId {
  groupId: number;
  applicationsIds: number[];

  constructor (data: IApplicationsIdsAndGroupId) {
    this.groupId = data.groupId
    this.applicationsIds = data.applicationsIds
  }
}