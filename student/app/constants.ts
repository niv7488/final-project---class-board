import { Headers, RequestOptions} from '@angular/http';

export const WEBSERVICE = "http://52.34.153.216:3000/";

var headers = new Headers({ 'Content-Type': 'application/json'});
export const OPTION = new RequestOptions({ headers: headers});

//Login Service
export const WEBSERVICE_LOGIN_PATH =  WEBSERVICE + "websiteLogin";

//Course List Service
export const WEBSERVICE_COURSE_LIST_GET_COURSE_BY_USER_ID_PATH =
    WEBSERVICE + 'userFullCourses';
export const WEBSERVICE_COURSE_LIST_GET_DATES_BY_COURSE_ID_PATH =
    WEBSERVICE + 'getCoursesContentDates';
export const WEBSERVICE_COURSE_LIST_GET_IMAGES_BY_DATE_PATH =
    WEBSERVICE + 'getDateImages';
