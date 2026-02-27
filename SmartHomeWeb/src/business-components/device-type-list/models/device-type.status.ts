import ListItem from "../../../components/list/models/ListItem";

export default interface DeviceTypeStatus{
    loading?: true;
    devicesTypes: Array<ListItem>;
    error?: string;
}