import DropdownOption from "../../../components/dropdown/models/DropdownOption";

export default interface HomeDeviceStatus {
    loading?: true;
    devices: Array<DropdownOption>;
    error?: string;
}