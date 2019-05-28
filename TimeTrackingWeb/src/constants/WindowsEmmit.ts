export enum GroupEmitEnum {
  ADD_GROUP = 'ADD_GROUP',
  EDIT_GROUP = 'EDIT_GROUP',
  CHANGE_GROUP_SUCCESS = 'CHANGE_GROUP_SUCCESS',
}

export enum ApplicationEmitEnum {
  EDIT_APPLICATION = 'EDIT_APPLICATION',
  CHANGE_APPLICATION_SUCCESS = 'CHANGE_APPLICATION_SUCCESS',
}

export enum ApplicationTitleEmitEnum {
  ADD_APPLICATION_TITLE = 'ADD_APPLICATION_TITLE',
  EDIT_APPLICATION_TITLE = 'EDIT_APPLICATION_TITLE',
  CHANGE_APPLICATION_TITLE = 'CHANGE_APPLICATION_TITLE',
}

export enum GroupApplicationTitleEmitEnum {
  ADD_GROUP_APPLICATION_TITLE = 'ADD_GROUP_APPLICATION_TITLE',
  EDIT_GROUP_APPLICATION_TITLE = 'EDIT_GROUP_APPLICATION_TITLE',
  CHANGE_GROUP_APPLICATION_TITLE = 'CHANGE_GROUP_APPLICATION_TITLE',
}

export enum ApplicationGroupEmitEnum {
  ADD_APPLICATION_GROUP = 'ADD_APPLICATION_GROUP',
  EDIT_APPLICATION_GROUP = 'EDIT_APPLICATION_GROUP',
  CHANGE_APPLICATION_GROUP_SUCCESS = 'CHANGE_APPLICATION_GROUP_SUCCESS',
}

export enum StaffEmitEnum {
  ADD_STAFF_TO_GROUP = 'ADD_STAFF_TO_GROUP',
  EDIT_STAFF = 'EDIT_STAFF',
  CHANGE_STAFF_SUCCESS = 'CHANGE_STAFF_SUCCESS',
}

export interface IApplicationsIdsAndGroupId {
  groupId: number;
  applicationsIds?: number[];
}

export class ApplicationsIdsAndGroupId implements IApplicationsIdsAndGroupId {
  groupId: number;
  applicationsIds?: number[];

  constructor (data: IApplicationsIdsAndGroupId) {
    this.groupId = data.groupId
    this.applicationsIds = data.applicationsIds
  }
}

export interface IApplicationTitlesIdsAndApplicationId {
  applicationId: number;
  applicationTitlesIds: number[];
}

export class ApplicationTitlesIdsAndApplicationId implements IApplicationTitlesIdsAndApplicationId {
  applicationId: number;
  applicationTitlesIds: number[];

  constructor (data: IApplicationTitlesIdsAndApplicationId) {
    this.applicationId = data.applicationId
    this.applicationTitlesIds = data.applicationTitlesIds
  }
}

export interface IStaffIdsAndGroupId {
  groupId: number;
  staffIds: number[];
}

export class StaffIdsAndGroupId implements IStaffIdsAndGroupId {
  groupId: number;
  staffIds: number[];

  constructor (data: IStaffIdsAndGroupId) {
    this.groupId = data.groupId
    this.staffIds = data.staffIds
  }
}
