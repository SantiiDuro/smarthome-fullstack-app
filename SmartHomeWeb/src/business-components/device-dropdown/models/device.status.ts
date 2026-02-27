import DropdownOption from "../../../components/dropdown/models/DropdownOption";

export default interface DeviceStatus {
    loading?: true;
    devices: Array<DropdownOption>;
    error?: string;
}