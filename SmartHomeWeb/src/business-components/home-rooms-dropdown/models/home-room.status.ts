import DropdownOption from "../../../components/dropdown/models/DropdownOption";

export default interface HomeRoomStatus {
    loading?: true;
    rooms: Array<DropdownOption>;
    error?: string;
}