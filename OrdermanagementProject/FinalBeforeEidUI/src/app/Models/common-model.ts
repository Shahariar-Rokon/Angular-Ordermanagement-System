export interface CommonModel {
}
export interface LoginRequestModel{
    Email:string,
    Password:string,
}
export interface LoginResult {
    success: boolean;
    message: string;
    token: string | null;
  }
  
export interface Status{
    statusCode:number,
    message:string
}

export interface RegisterDTO{
    name:string;
    email:string;
    passWord:string;
    confirmPassword:string
}
export interface GeneralResponse {
    success: boolean;
    message: string;
  }